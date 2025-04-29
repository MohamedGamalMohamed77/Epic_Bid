using Epic_Bid.Core.Domain.Entities.Products;
using Epic_Bid.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Abstraction.Services.AuctionServ
{
    public interface IAuctionService
    {
        Task<bool> PlaceBidAsync(PlaceBidRequestDto request);
        // Get Bid For product
        Task<IReadOnlyList<AuctionForProductDto>> GetBidsForProductAsync(int productId);
        Task CloseAuctionAsync(int productId);

        public Task SendEmailToWinner(int productid, string subject, EmailWinnerDataDto emailWinnerDataDto);
    }
}
