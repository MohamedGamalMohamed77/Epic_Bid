using Epic_Bid.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Domain.Entities.Products
{
    public class AuctionBid : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty; // اسم المستخدم
        public int ProductId { get; set; } // مرتبط بالمنتج
        public Product Product { get; set; } = null!; // المنتج المرتبط بالمزايدة
        public decimal BidAmount { get; set; } // المبلغ المزايد عليه
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    }

}
