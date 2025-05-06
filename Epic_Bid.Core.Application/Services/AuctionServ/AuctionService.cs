using AutoMapper;
using Epic_Bid.Core.Application.Abstraction.Services.AuctionServ;
using Epic_Bid.Core.Application.Abstraction.Services.Auth;
using Epic_Bid.Shared.Exceptions;
using Epic_Bid.Core.Application.SpecificationImplementation.AuctionSpec;
using Epic_Bid.Core.Domain.Contracts.Persistence;
using Epic_Bid.Core.Domain.Entities.Products;
using Epic_Bid.Shared;
using Hangfire;

namespace Epic_Bid.Core.Application.Services.AuctionServ
{
	public class AuctionService(IUnitOfWork _unitOfWork, IMapper _mapper, IEmailService _emailService) : IAuctionService
    {
        #region Place Bid
        public async Task<bool> PlaceBidAsync(PlaceBidRequestDto request)
        {
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(request.ProductId);

            if (product == null)
                throw new NotFoundException(nameof(Product), request.ProductId);
            if (product.IsAuctionClosed)
                throw new BadRequestException("Auction closed");

            if (!product.IsAuction)
                throw new BadRequestException("Product is not an auction");

            var now = DateTime.UtcNow;
            if (now < product?.AuctionStartTime!.Value)
                throw new BadRequestException($"Auction will start at {product.AuctionStartTime!.Value}");

            if (now > product!.AuctionEndTime!.Value)
                throw new BadRequestException($"Auction ended at {product.AuctionEndTime.Value}");

            if (request.BidAmount <= (product.CurrentBid ?? product.Price))
                throw new BadRequestException("Bid must be higher than current bid");

            // user name and user id from token
            var bid = new AuctionBid
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                BidAmount = request.BidAmount,
                UserName = request.UserName
            };
            try
            {
                await _unitOfWork.GetRepository<AuctionBid>().AddAsync(bid);

                product.CurrentBid = request.BidAmount;
                product.CurrentWinnerUserId = request.UserId;
                // IMPORTANT: tell the context that you updated the product
                //_unitOfWork.GetRepository<Product>().Update(product);
                await _unitOfWork.SaveChangesAsync();
            }catch(Exception ex)
            {
                throw new BadRequestException(ex.Message);
            }
           
            return true;

        }
        #endregion

        #region Close Bid
        public async Task CloseAuctionAsync(int productId)
        {
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
            if(product == null)
                throw new NotFoundException(nameof(Product), productId);

            if ( !product.IsAuction || product.IsAuctionClosed)
                return;

            if (DateTime.UtcNow < product.AuctionEndTime)
                return; // مزاد لسه ما انتهاش

            product.IsAuctionClosed = true;

            //// هنا ممكن ترسل إيميل أو إشعار للفائز لو عايز (اختياري)
            //var bids = await GetBidsForProductAsync(productId);
            //if (bids == null || bids.Count == 0)
            //    throw new BadRequestException("No bids found for this product");
            ////Name of The Winner
            //var emailWinnerDataDto = new EmailWinnerDataDto();
            //var winnerBidName = bids.FirstOrDefault()?.UserName;
            //emailWinnerDataDto.Username = winnerBidName!;
            //emailWinnerDataDto.Finlaprice = bids.FirstOrDefault()!.BidAmount;
            ////Email Of The Winner to send the email
            //var winnerBidEmail = await _unitOfWork.GetRepository<AuctionBid>().GetUserByIdAsycn(bids.FirstOrDefault()!.UserId);
            //// send the email 
            //await _emailService.SendEmailToWinnerAsync(winnerBidEmail!.Email!, "Winner!", emailWinnerDataDto);


            await _unitOfWork.SaveChangesAsync();
        }

        #endregion

        #region GetBidsForProductAsync
        public async Task<IReadOnlyList<AuctionForProductDto>> GetBidsForProductAsync(int productId)
        {
            var spec = new GetBidForProduct(productId);
            var bids = await _unitOfWork.GetRepository<AuctionBid>().GetAllAsync(spec);
            if (bids == null || bids.Count == 0)
                throw new BadRequestException("No bids found for this product");
            var mappedbids = _mapper.Map<IReadOnlyList<AuctionBid>, IReadOnlyList<AuctionForProductDto>>(bids);
            return mappedbids;


        }
        #endregion

        #region SenEmailToTheWinner
        //هنا ممكن تضيف كود لإرسال إيميل للفائز بالمزاد
        [AutomaticRetry(Attempts = 0)]
        public  async Task SendEmailToWinner(int productid, string subject,  EmailWinnerDataDto emailWinnerDataDto)
        {
            Console.WriteLine("SendEmailToWinner");

            var bids = await GetBidsForProductAsync(productid);
            if (bids == null || bids.Count == 0)
                throw new BadRequestException("No bids found for this product");
            //Name of The Winner
            var winnerBidName = bids.FirstOrDefault()?.UserName;
            emailWinnerDataDto.Username = winnerBidName!;
            emailWinnerDataDto.Finlaprice = bids.FirstOrDefault()!.BidAmount;
            //Email Of The Winner to send the email
            var winnerBidId = bids.FirstOrDefault()!.UserId;
            var WinnerBid = await _unitOfWork.GetRepository<AuctionBid>().GetUserByIdAsync(winnerBidId);
            var WinnerEmail = WinnerBid!.Email;
            if (WinnerEmail == null)
            {
                throw new BadRequestException("No email found for this user");
            }
            // send the email 
            await _emailService.SendEmailToWinnerAsync(WinnerEmail, "Winner!", emailWinnerDataDto);

        }
        #endregion
    }
}
