using Epic_Bid.Apis.Controllers.Controllers.Base;
using Epic_Bid.Apis.Controllers.Controllers.Errors;
using Epic_Bid.Apis.Controllers.UploadImageHandlerExtension;
using Epic_Bid.Core.Application.Abstraction.Models.ProductDt;
using Epic_Bid.Core.Application.Abstraction.Services;
using Epic_Bid.Core.Application.Abstraction.Services.Auth;
using Epic_Bid.Core.Domain.Entities.Order;
using Epic_Bid.Core.Domain.Entities.Products;
using Epic_Bid.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Apis.Controllers.Controllers.Prod
{
    public class ProductController(IServiceManager _serviceManager, IAuthService _authService) : BaseApiController
    {
        #region GetAllProducts
        // GetAllProductsAsync  
        [HttpGet("GetAllProducts")]
        [ProducesResponseType(typeof(IReadOnlyList<DeliveryMethod>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAllProducts([FromQuery] ProductParamQuery? param)
        {
            var Products = await _serviceManager.ProductService.GetAllProductsAsync(param);
            return Ok(Products);
        }
        #endregion

        #region GetProductById
        // GetProductByIdAsync
        [HttpGet("GetProductById")]
        [ProducesResponseType(typeof(IReadOnlyList<DeliveryMethod>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductByIdDto>> GetProductById(int id)
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
        [ProducesResponseType(typeof(IReadOnlyList<DeliveryMethod>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<ReviewDto>>> GetReviewOfProductId(int ProductId)
        {
            var Reviews = await _serviceManager.ProductService.GetReviewsOfProductId(ProductId);
            return Ok(Reviews);
        }
        #endregion

        #region AddProduct
        // AddProduct
        [HttpPost("AddProduct")]
        [ProducesResponseType(typeof(IReadOnlyList<DeliveryMethod>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult<CreateProductDto>> AddProduct(CreateProductDto CreatedProduct)
        {
            //get the user id by claim
            var user = await _authService.GetCurrentUser(User);
            var UserId = user.Id;
            if (string.IsNullOrEmpty(UserId))
            {
                return Unauthorized("User not found");
            }
            if (CreatedProduct.IsAuction)
            {
                if (CreatedProduct.AuctionStartTime == null || CreatedProduct.AuctionEndTime == null)
                    return BadRequest("Auction time must be provided for auction products.");

                if (CreatedProduct.AuctionEndTime <= CreatedProduct.AuctionStartTime)
                    return BadRequest("Auction end time must be after start time.");
            }
            // Add the ImageFile
            //CreatedProduct.ImageUrl = UploadImageHandler.UploadImage(CreatedProduct.ImageUploaded);
            var Product = await _serviceManager.ProductService.AddProductAsync(CreatedProduct, UserId);
            return Ok(Product);
        }
        #endregion

        #region Update Product
        // AddProduct
        [HttpPut("UpdateProduct")]
        [ProducesResponseType(typeof(IReadOnlyList<DeliveryMethod>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult<CreateProductDto>> UpdateProduct(CreateProductDto updateProduct)
        {
            var user = await _authService.GetCurrentUser(User);
            var userId = user.Id;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated or token is invalid");
            }
            //updateProduct.ImageUrl = UploadImageHandler.UploadImage(updateProduct.ImageUploaded);
            var product = await _serviceManager.ProductService.UpdateProductAsync(updateProduct, userId);
            return Ok(product);
        }

        #endregion

        #region Delete Product
        [HttpDelete("DeleteProduct")]
        [ProducesResponseType(typeof(IReadOnlyList<DeliveryMethod>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var user = await _authService.GetCurrentUser(User);
            var userId = user.Id;
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
        [ProducesResponseType(typeof(IReadOnlyList<DeliveryMethod>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult<AddReviewDto>> AddReview(AddReviewDto reviewDto)
        {
            var user = await _authService.GetCurrentUser(User);
            var userId = user.Id;
            var UserName = user.DisplayName;

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