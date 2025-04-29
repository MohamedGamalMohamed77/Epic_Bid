using AutoMapper;
using Epic_Bid.Core.Application.Abstraction.Models.Auth;
using Epic_Bid.Core.Application.Abstraction.Models.ProductDt;
using Epic_Bid.Core.Application.Abstraction.Services.AuctionServ;
using Epic_Bid.Core.Application.Abstraction.Services.Auth;
using Epic_Bid.Core.Application.Abstraction.Services.IProductServ;
using Epic_Bid.Core.Application.Exceptions;
using Epic_Bid.Core.Application.Services.AuctionServ;
using Epic_Bid.Core.Application.SpecificationImplementation;
using Epic_Bid.Core.Domain.Contracts.Persistence;
using Epic_Bid.Core.Domain.Entities.Auth;
using Epic_Bid.Core.Domain.Entities.Products;
using Epic_Bid.Core.Domain.Specifications;
using Epic_Bid.Shared;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Services.ProductServ
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper _mapper, IBackgroundJobClient _backgroundJobClient) : IProductService
    {
        #region AddProduct  
        ///public async Task<CreateProductDto> AddProductAsync(CreateProductDto CreatedProduct, string UserId)
        //{
        //    if (CreatedProduct == null)
        //    {
        //        throw new BadRequestException("Product data is null");
        //    }
        //    ValidationOnTimeAuction(CreatedProduct);
        //    // نعمل مابنج للبرودكت الجديد على البرودكت القديم

        //    var Product = _mapper.Map<CreateProductDto, Product>(CreatedProduct);
        //    if (Product == null)
        //    {
        //        throw new BadRequestException("Product mapping failed");
        //    }
        //    Product.UserCreatedId = UserId;

        //    try
        //    {
        //        var category = await unitOfWork.GetRepository<ProductCategory>().GetByIdAsync(CreatedProduct.ProductCategoryId);
        //        if (category == null)
        //        {
        //            throw new NotFoundException("Category", CreatedProduct.ProductCategoryId);
        //        }

        //        await unitOfWork.GetRepository<Product>().AddAsync(Product);
        //        await unitOfWork.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BadRequestException(ex.Message);
        //    }
        //    CreatedProduct.Id = Product.Id;
        //    HangFire(CreatedProduct, Product);
        //    ///if (CreatedProduct.AuctionStartTime != null && CreatedProduct.AuctionEndTime != null && CreatedProduct.AuctionStartTime < CreatedProduct.AuctionEndTime && DateTime.UtcNow > CreatedProduct.AuctionEndTime && Product.IsAuction)
        //    //{
        //    //    // make hangfire job to open the auction, and send email to the user
        //    //    var TimeDelay = (CreatedProduct.AuctionEndTime - CreatedProduct.AuctionStartTime).Value.TotalMinutes;
        //    //    var jobId = _backgroundJobClient.Schedule<IAuctionService>(x => x.CloseAuctionAsync(Product.Id), TimeSpan.FromMinutes(TimeDelay));
        //    //var emailwinnerdata = new EmailWinnerDataDto
        //    //{
        //    //    Productname = CreatedProduct.Name,
        //    //    Finlaprice = CreatedProduct.CurrentBid ?? 0,
        //    //    AuctionEndDate = CreatedProduct.AuctionEndTime.Value,
        //    //    Username = ""
        //    //};
        //    //var emailJobId = _backgroundJobClient.Schedule<IAuctionService>(x => x.SendEmailToWinner(Product.Id, "Winner!", emailwinnerdata), TimeSpan.FromMinutes(TimeDelay));
        //    //}
        //    return CreatedProduct;
        //}
        public async Task<CreateProductDto> AddProductAsync(CreateProductDto CreatedProduct, string UserId)
        {
            if (CreatedProduct == null)
            {
                throw new BadRequestException("Product data is null");
            }

            ValidationOnTimeAuction(CreatedProduct);

            var Product = _mapper.Map<CreateProductDto, Product>(CreatedProduct);
            if (Product == null)
            {
                throw new BadRequestException("Product mapping failed");
            }

            Product.UserCreatedId = UserId;

            try
            {
                var category = await unitOfWork.GetRepository<ProductCategory>().GetByIdAsync(CreatedProduct.ProductCategoryId);
                if (category == null)
                {
                    throw new NotFoundException("Category", CreatedProduct.ProductCategoryId);
                }

                await unitOfWork.GetRepository<Product>().AddAsync(Product);
                await unitOfWork.SaveChangesAsync();

                // Call Hangfire and update product with job IDs
                await ScheduleAuctionJobsAsync(CreatedProduct, Product);

                // Save job IDs into DB
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.Message);
            }

            CreatedProduct.Id = Product.Id;
            return CreatedProduct;
        }

        ///private void HangFire(CreateProductDto CreatedProduct, Product Product)
        //{
        //    if (Product.IsAuction &&
        //                    CreatedProduct.AuctionStartTime != null &&
        //                    CreatedProduct.AuctionEndTime != null &&
        //                    CreatedProduct.AuctionStartTime < CreatedProduct.AuctionEndTime)
        //    {
        //        // تحقق إذا كان المزاد لم ينته بعد
        //        if (DateTime.UtcNow < CreatedProduct.AuctionEndTime)
        //        {
        //            // حساب المدة المتبقية حتى نهاية المزاد
        //            var timeRemaining = CreatedProduct.AuctionEndTime.Value - DateTime.UtcNow;

        //            // جدولة إغلاق المزاد عند وقت الانتهاء
        //            var jobId = _backgroundJobClient.Schedule<IAuctionService>(
        //                                 x => x.CloseAuctionAsync(Product.Id), timeRemaining);

        //            //// يمكنك أيضاً إرسال إشعار ببدء المزاد
        //            var emailwinnerdata = new EmailWinnerDataDto
        //            {
        //                Productname = CreatedProduct.Name,
        //                Finlaprice = CreatedProduct.CurrentBid ?? 0,
        //                AuctionEndDate = CreatedProduct.AuctionEndTime.Value,
        //                Username = ""
        //            };
        //            var emailJobId = _backgroundJobClient.Schedule<IAuctionService>(x => x.SendEmailToWinner(Product.Id, "Winner!", emailwinnerdata), timeRemaining);

        //            //BackgroundJob.Enqueue<IAuctionService>(
        //            //    x => x.NotifyAuctionStartedAsync(Product.Id));
        //        }

        //    }
        //}
        private async Task ScheduleAuctionJobsAsync(CreateProductDto CreatedProduct, Product product)
        {
            if (product.IsAuction &&
                CreatedProduct.AuctionStartTime != null &&
                CreatedProduct.AuctionEndTime != null &&
                CreatedProduct.AuctionStartTime < CreatedProduct.AuctionEndTime)
            {
                if (DateTime.UtcNow < CreatedProduct.AuctionEndTime)
                {
                    var timeRemaining = CreatedProduct.AuctionEndTime.Value - DateTime.UtcNow;

                    // Close auction job
                    var closeJobId = _backgroundJobClient.Schedule<IAuctionService>(
                        x => x.CloseAuctionAsync(product.Id), timeRemaining);

                    // Send email job
                    var emailData = new EmailWinnerDataDto
                    {
                        Productname = CreatedProduct.Name,
                        Finlaprice = CreatedProduct.CurrentBid ?? 0,
                        AuctionEndDate = CreatedProduct.AuctionEndTime.Value,
                        Username = ""
                    };

                    var emailJobId = _backgroundJobClient.Schedule<IAuctionService>(
                        x => x.SendEmailToWinner(product.Id, "Winner!", emailData), timeRemaining);

                    // Store job IDs in product
                    product.AuctionCloseJobId = closeJobId;
                    product.AuctionEmailJobId = emailJobId;
                }
            }
        }

        private static void ValidationOnTimeAuction(CreateProductDto CreatedProduct)
        {
            if (CreatedProduct.IsAuction)
            {
                if (CreatedProduct.AuctionStartTime == null || CreatedProduct.AuctionEndTime == null)
                {
                    throw new BadRequestException("Auction time must be provided for auction products.");
                }

                var now = DateTime.UtcNow;

                if (now > CreatedProduct.AuctionStartTime.Value)
                {
                    throw new BadRequestException("Auction start time must be in the future.");
                }

                if (CreatedProduct.AuctionEndTime.Value <= CreatedProduct.AuctionStartTime.Value)
                {
                    throw new BadRequestException("Auction end time must be after start time.");
                }

                if (now > CreatedProduct.AuctionEndTime.Value)
                {
                    throw new BadRequestException("Auction end time must be in the future.");
                }


            }
        }
        #endregion

        #region UpdateProduct  
        ///public async Task<CreateProductDto> UpdateProductAsync(CreateProductDto UpdatedProduct, string UserId)
        //{
        //    if (UpdatedProduct == null)
        //    {
        //        throw new BadRequestException("Product data is null");
        //    }

        //    // نجيب البرودكت الحقيقي من الداتا بيز
        //    var existingProduct = await unitOfWork.GetRepository<Product>().GetByIdAsync(UpdatedProduct.Id);
        //    if (existingProduct == null)
        //    {
        //        throw new NotFoundException("Product", UpdatedProduct.Id);
        //    }


        //    if(existingProduct.UserCreatedId != UserId)
        //    {
        //        // throw exception 

        //       throw new UnAuthorizedException("مش البرودكت بتاعك يا نجم عشان تلعب فيه");
        //    }

        //    var category = await unitOfWork.GetRepository<ProductCategory>().GetByIdAsync(UpdatedProduct.ProductCategoryId);
        //    if (category == null)
        //    {
        //        throw new NotFoundException("Category", UpdatedProduct.ProductCategoryId);
        //    }

        //    if (UpdatedProduct.IsAuction && existingProduct.IsAuction == false)
        //    {
        //        ValidationOnTimeAuction(UpdatedProduct);
        //        HangFire(UpdatedProduct, existingProduct);
        //    }else if (UpdatedProduct.IsAuction && existingProduct.IsAuction == true)
        //    {
        //        if (existingProduct.IsAuctionClosed == false)
        //        {
        //            ValidationOnTimeAuction(UpdatedProduct);
        //            HangFire(UpdatedProduct, existingProduct);
        //        }
        //        else if(existingProduct.IsAuctionClosed == true)
        //        {
        //            ValidationOnTimeAuction(UpdatedProduct);
        //            // delete old job
        //            _backgroundJobClient.Delete(existingProduct.Id.ToString());

        //            HangFire(UpdatedProduct, existingProduct);
        //        }
        //    }

        //    try
        //    {
        //        // نعمل مابنج للبرودكت الجديد على البرودكت القديم
        //        _mapper.Map(UpdatedProduct, existingProduct);
        //        await unitOfWork.SaveChangesAsync();
        //    }
        //    catch (Exception)
        //    {
        //        throw new BadRequestException("Error while adding product");
        //    }
        //    return UpdatedProduct;


        //}
        public async Task<CreateProductDto> UpdateProductAsync(CreateProductDto UpdatedProduct, string UserId)
        {
            if (UpdatedProduct == null)
            {
                throw new BadRequestException("Product data is null");
            }

            var existingProduct = await unitOfWork.GetRepository<Product>().GetByIdAsync(UpdatedProduct.Id);
            if (existingProduct == null)
            {
                throw new NotFoundException("Product", UpdatedProduct.Id);
            }

            if (existingProduct.UserCreatedId != UserId)
            {
                throw new UnAuthorizedException("مش البرودكت بتاعك يا نجم عشان تلعب فيه");
            }

            var category = await unitOfWork.GetRepository<ProductCategory>().GetByIdAsync(UpdatedProduct.ProductCategoryId);
            if (category == null)
            {
                throw new NotFoundException("Category", UpdatedProduct.ProductCategoryId);
            }

            bool needsNewJobs = false;

            if (UpdatedProduct.IsAuction)
            {
                ValidationOnTimeAuction(UpdatedProduct);

                if (existingProduct.IsAuction == false || existingProduct.IsAuctionClosed == false)
                {
                    // Cancel old jobs if they exist
                    if (!string.IsNullOrEmpty(existingProduct.AuctionCloseJobId))
                    {
                        _backgroundJobClient.Delete(existingProduct.AuctionCloseJobId);
                        existingProduct.AuctionCloseJobId = null;
                    }

                    if (!string.IsNullOrEmpty(existingProduct.AuctionEmailJobId))
                    {
                        _backgroundJobClient.Delete(existingProduct.AuctionEmailJobId);
                        existingProduct.AuctionEmailJobId = null;
                    }

                    needsNewJobs = true;
                }
            }

            try
            {
                _mapper.Map(UpdatedProduct, existingProduct);

                if (needsNewJobs)
                {
                    await ScheduleAuctionJobsAsync(UpdatedProduct, existingProduct);
                }

                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new BadRequestException("Error while updating product");
            }

            return UpdatedProduct;
        }

        #endregion

        #region AddReview  
        public async Task AddReviewAsync(AddReviewDto AddedReview, string UserId, string UseraName)
        {
            if (AddedReview == null)
            {
                throw new BadRequestException("Review data is null");
            }
            var MappedReview = _mapper.Map<AddReviewDto, CustomerReview>(AddedReview);
            if (MappedReview == null)
            {
                throw new BadRequestException("Review mapping failed");
            }
            MappedReview.UserId = UserId;
            MappedReview.UserName = UseraName; 

            try
            {
                
                await unitOfWork.GetRepository<CustomerReview>().AddAsync(MappedReview);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"Error while adding Review : {ex}");
            }
            
           
        }
        #endregion

        #region DeleteProduct  
        ///public async Task<bool> DeleteProductAsync(int productId, string userId)
        //{
        //    // نجيب المنتج
        //    var product = await unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
        //    if (product == null)
        //    {
        //        throw new NotFoundException("Product", productId);
        //    }

        //    // نتأكد ان اليوزر هو صاحب المنتج
        //    if (product.UserCreatedId != userId)
        //    {
        //        throw new UnAuthorizedException("مش البرودكت بتاعك يا نجم عشان تمسحه");
        //    }

        //    // نحذفه
        //    try
        //    {
        //        unitOfWork.GetRepository<Product>().Delete(product);
        //        await unitOfWork.SaveChangesAsync();
        //    }catch(Exception ex)
        //    {
        //        throw new BadRequestException($"Error while deleting product: { ex.Message}");
        //    }

        //    return true;
        //}
        public async Task<bool> DeleteProductAsync(int productId, string userId)
        {
            var product = await unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
            if (product == null)
            {
                throw new NotFoundException("Product", productId);
            }

            if (product.UserCreatedId != userId)
            {
                throw new UnAuthorizedException("مش البرودكت بتاعك يا نجم عشان تمسحه");
            }

            try
            {
                // حذف الـ jobs لو موجودين
                if (!string.IsNullOrEmpty(product.AuctionCloseJobId))
                {
                    _backgroundJobClient.Delete(product.AuctionCloseJobId);
                }

                if (!string.IsNullOrEmpty(product.AuctionEmailJobId))
                {
                    _backgroundJobClient.Delete(product.AuctionEmailJobId);
                }

                // حذف المنتج
                unitOfWork.GetRepository<Product>().Delete(product);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"Error while deleting product: {ex.Message}");
            }

            return true;
        }


        #endregion

        #region GetAllCategories  
        public async Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync()
        {
            var Categories = await unitOfWork.GetRepository<ProductCategory>().GetAllAsync();
            if (Categories == null || !Categories.Any())
            {
                throw new NotFoundException("categories", "any");
            }
            return _mapper.Map<IReadOnlyList<CategoryDto>>(Categories);
        }
        #endregion

        #region GetAllReviews  
        public Task<IReadOnlyList<ReviewDto>> GetAllReviewsAsync(ISpecification<CustomerReview> Specification)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region GetAllProducts  
        public async Task<ProductPagination<ProductDto>> GetAllProductsAsync(ProductParamQuery? param)
        {
            var spec = new ProductFilteration(param);
            var Products = await unitOfWork.GetRepository<Product>().GetAllAsync(spec);
            if (Products == null || !Products.Any())
            {
                throw new NotFoundException("products", "any");
            }
            var Data = _mapper.Map<IReadOnlyList<ProductDto>>(Products);
            // Page Size  
            var AllProductCount = Products.Count();
            // Count Of All Data   
            var Count = await unitOfWork.GetRepository<Product>().GetCountAsync(new ProductCountSpec(param));
            return new ProductPagination<ProductDto>(param!.PageSize, param.PageIndex, Count, Data);
        }
        #endregion

        #region GetProductById  
        public async Task<ProductByIdDto> GetProductByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid product ID");
            }
            var spec = new ProductWithCategorySpecifcation(id);
            // Getting the product
            var Product = await unitOfWork.GetRepository<Product>().GetByIdAsync(spec);
            if (Product == null)
            {
                throw new NotFoundException("product", id);
            }
            //Get Average Ratings
            var Reviews = await unitOfWork.GetRepository<CustomerReview>().GetAllAsync(new ReviewByIdAndProductId(id));
            var MappedProduct = _mapper.Map<ProductByIdDto>(Product);
            if(Product.Reviews is not null && Product.Reviews.Any())
            {
                MappedProduct.AverageRating = Reviews.Average(x => x.Rating); 
                MappedProduct.TotalRatings = Reviews.Count;
            }else
            {
                MappedProduct.AverageRating = 0;
                MappedProduct.TotalRatings = 0;
            }
                return MappedProduct;
        }
        #endregion

        #region GetReviewsOfProductId  
        public async Task<IReadOnlyList<ReviewDto>> GetReviewsOfProductId(int ProductId)
        {
            var product = await unitOfWork.GetRepository<Product>().GetByIdAsync(ProductId);
            if(product == null)
            {
                throw new NotFoundException("Product", ProductId);
            }
            var spec = new ReviewByIdAndProductId(ProductId);
            var Reviews = await unitOfWork.GetRepository<CustomerReview>().GetAllAsync(spec);
            if (Reviews == null)
            {
                throw new NotFoundException("reviews for product", ProductId);
            }
            return _mapper.Map<IReadOnlyList<ReviewDto>>(Reviews);
        }
        #endregion

    }
}
