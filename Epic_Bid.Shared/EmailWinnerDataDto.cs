using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Shared
{
    public class EmailWinnerDataDto
    {
        public  string  Username  { get; set; } = null!;
        public string   Productname { get; set; } = null!;
        public decimal  Finlaprice { get; set; }
        public DateTime AuctionEndDate { get; set; }

    }
}
