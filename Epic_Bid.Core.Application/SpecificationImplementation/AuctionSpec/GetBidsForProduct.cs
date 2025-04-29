using Epic_Bid.Core.Domain.Entities.Products;
using Epic_Bid.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.SpecificationImplementation.AuctionSpec
{
    public class GetBidForProduct : BaseSpecification<AuctionBid>
    {
        public GetBidForProduct(int productid) : base(p => p.ProductId == productid)
        {
            AddOrderByDescending(p => p.BidAmount);
        }
    }
}
