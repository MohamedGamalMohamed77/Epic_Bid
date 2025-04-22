using Epic_Bid.Core.Application.Abstraction.Models.ProductDt;
using Epic_Bid.Core.Domain.Entities.Products;
using Epic_Bid.Core.Domain.Specifications;
using Epic_Bid.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Abstraction.Services.IProductServ
{
    public interface IProductService
    {
        // Add Product
        Task<CreateProductDto> AddProductAsync(CreateProductDto CreatedProduct ,string UserId);
        // Update Product
        Task<CreateProductDto> UpdateProductAsync(CreateProductDto productDto,string userid);
        // Delete Product
        Task<bool> DeleteProductAsync(int ProductId,string UserId);
        //Add Review
        Task AddReviewAsync(AddReviewDto reviewDto, string UserId ,string UserName);

        //Get All Products
        Task<ProductPagination<ProductDto>> GetAllProductsAsync(ProductParamQuery? param);
        //Get Product By Id
        Task<ProductByIdDto> GetProductByIdAsync(int id);

        // Categeroy
        Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync();
        // CustomerReview
        Task<IReadOnlyList<ReviewDto>> GetReviewsOfProductId(int ProductId);

       
    }
}
