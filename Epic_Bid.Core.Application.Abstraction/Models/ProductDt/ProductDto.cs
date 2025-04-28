using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Abstraction.Models.ProductDt
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Color { get; set; } = null!;
        public bool IsAuction { get; set; } = false; // هل المنتج معروض كمزاد؟
        public DateTime? AuctionStartTime { get; set; } // بداية المزاد
        public DateTime? AuctionEndTime { get; set; } // نهاية المزاد
        public bool IsAuctionClosed { get; set; } = false; // هل المزاد انتهى؟ 
    }
}
