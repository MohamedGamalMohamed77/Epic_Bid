using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Shared
{
    public class PlaceBidRequestDto
    {
        public int ProductId { get; set; }
        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty; // اسم المستخدم
        public decimal BidAmount { get; set; }
    }
}
