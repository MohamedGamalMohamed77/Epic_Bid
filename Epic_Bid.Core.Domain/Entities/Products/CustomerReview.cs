using Epic_Bid.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Domain.Entities.Products
{
    public class CustomerReview : BaseEntity
    {
        public int ProductId { get; set; } // foreign key
        public Product Product { get; set; } // navigation property
        public string UserId { get; set; } = string.Empty; // foreign key , this should be the id of the user who wrote the review
        public string UserName { get; set; } = string.Empty; // name of the user who wrote the review
        public string ReviewText { get; set; } = string.Empty;
        public int Rating { get; set; } // rating out of 5
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
