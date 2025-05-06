using Epic_Bid.Apis.Controllers.Controllers.Base;
using Epic_Bid.Apis.Controllers.Controllers.Errors;
using Epic_Bid.Core.Application.Abstraction.Services.AuctionServ;
using Epic_Bid.Core.Application.Abstraction.Services.Auth;
using Epic_Bid.Core.Application.Abstraction.Services.IProductServ;
using Epic_Bid.Core.Domain.Entities.Order;
using Epic_Bid.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Apis.Controllers.Controllers.Auct
{
    public class AuctionController(IAuctionService _auctionService ,IProductService productService,IAuthService authService): BaseApiController
    {
        [HttpPost("Bid")]
        [ProducesResponseType(typeof(IReadOnlyList<DeliveryMethod>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PlaceBid([FromBody] PlaceBidRequestDto request)
        {
            //var userId = HttpContext.User.FindFirstValue(ClaimTypes.GroupSid);
            //var username = HttpContext.User.FindFirstValue(ClaimTypes.GivenName);
            var user = await authService.GetCurrentUser(User);
            if (string.IsNullOrEmpty(user.Id) && string.IsNullOrEmpty(user.DisplayName))
            {
                return Unauthorized("User not authenticated." );
            }
            request.UserId = user.Id;
            request.UserName = user.DisplayName;
            try
            {
                await _auctionService.PlaceBidAsync(request);
                return Ok( "Bid placed successfully." );
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        //get auction for product
        [HttpGet("GetAuctionForProduct")]
        [ProducesResponseType(typeof(IReadOnlyList<DeliveryMethod>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAuctionForProduct(int productId)
        {
            // if action is closed, show the winner

            var product = await productService.GetProductByIdAsync(productId);
            if(product == null)
            {
                return NotFound( "Product not found.");
            }

            var auction = await _auctionService.GetBidsForProductAsync(productId);
            if (auction == null || auction.Count == 0)
            {
                return NotFound("No bids found for this product." );
            }
            // check if the auction is closed
            if (product.IsAuctionClosed)
            {
                return Ok(new { message = "Auction is closed.", winner = auction.FirstOrDefault()!.UserName, bidAmount = auction.FirstOrDefault()!.BidAmount ,auction});
            }

            return Ok(auction);
        }

        //[HttpPost("schedule-close/{productId}")]
        //public IActionResult ScheduleAuctionClose(int productId)
        //{
        //    // اسند Job لقفل المزاد
        //    _backgroundJobClient.Schedule<IAuctionService>(
        //        x => x.CloseAuctionAsync(productId),
        //        TimeSpan.FromMinutes(1)); // مثال: بعد دقيقة (هنا انت المفروض تحسب المدة صح بناءً على نهاية المزاد)

        //    return Ok(new { message = "Auction close scheduled." });
        //}
        // get Bid For product


        [HttpPost("force-close/{productId}")]
        public async Task<IActionResult> ForceCloseAuction(int productId)
        {
            await _auctionService.CloseAuctionAsync(productId);
            return Ok( "Auction closed manually." );
        }
       

    }
}
