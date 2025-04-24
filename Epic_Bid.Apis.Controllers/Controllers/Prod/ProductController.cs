using Epic_Bid.Apis.Controllers.Controllers.Base;
using Epic_Bid.Apis.Controllers.UploadImageHandlerExtension;
using Epic_Bid.Core.Application.Abstraction.Models.ProductDt;
using Epic_Bid.Core.Application.Abstraction.Services;
using Epic_Bid.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Apis.Controllers.Controllers.Prod
{
    public class ProductController(IServiceManager _serviceManager) : BaseApiController
    {
        #region GetAllProducts
        // GetAllProductsAsync  
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAllProducts([FromQuery] ProductParamQuery? param)
        {
            var Products = await _serviceManager.ProductService.GetAllProductsAsync(param);
            return Ok(Products);
        }
        #endregion

        #region GetProductById
        // GetProductByIdAsync
        [HttpGet("GetProductById")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var Product = await _serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(Product);
        }
        #endregion

        #region GetAllCategories
        // GetAllCategoriesAsync
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetAllCategories()
        {
            var Categories = await _serviceManager.ProductService.GetAllCategoriesAsync();
            return Ok(Categories);
        }
        #endregion

        #region GetReviewOfProductId
        // GetReviewOfProductId
        [HttpGet("GetReviewOfProductId")]
        public async Task<ActionResult<IReadOnlyList<ReviewDto>>> GetReviewOfProductId(int ProductId)
        {
            var Reviews = await _serviceManager.ProductService.GetReviewsOfProductId(ProductId);
            return Ok(Reviews);
        }
        #endregion

        #region AddProduct
        // AddProduct
        [HttpPost("AddProduct")]
        [Authorize]
        public async Task<ActionResult<CreateProductDto>> AddProduct( CreateProductDto CreatedProduct)
        {
            //get the user id by claim
            var UserId = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)?.Value;
            if (string.IsNullOrEmpty(UserId))
            {
                return Unauthorized("User not found");
            }
            // Add the ImageFile
            CreatedProduct.ImageUrl = UploadImageHandler.UploadImage(CreatedProduct.ImageUploaded);
            var Product = await _serviceManager.ProductService.AddProductAsync(CreatedProduct, UserId);
            return Ok(Product);
        }
        #endregion

        #region Update Product
        // AddProduct
        [HttpPut("UpdateProduct")]
        [Authorize]
        public async Task<ActionResult<CreateProductDto>> UpdateProduct(CreateProductDto updateProduct)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated or token is invalid");
            }
            updateProduct.ImageUrl = UploadImageHandler.UploadImage(updateProduct.ImageUploaded);
            var product = await _serviceManager.ProductService.UpdateProductAsync(updateProduct, userId);
            return Ok(product);
        }

        #endregion

        #region Delete Product
        [HttpDelete("DeleteProduct")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not found");
            }

            await _serviceManager.ProductService.DeleteProductAsync(productId, userId);
            return Ok("Product deleted successfully");
        }
        #endregion

        #region Add Review
        [HttpPost("AddReview")]
        [Authorize]
        public async Task<ActionResult<AddReviewDto>> AddReview(AddReviewDto reviewDto)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.PrimarySid)?.Value;
            var UserName = HttpContext.User.FindFirst(ClaimTypes.GivenName)?.Value;

            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(UserName))
            {
                return Unauthorized("User not found");
            }
            await _serviceManager.ProductService.AddReviewAsync(reviewDto, userId!, UserName!);
            return Ok("Review added successfully");
        }
        #endregion
    }
}