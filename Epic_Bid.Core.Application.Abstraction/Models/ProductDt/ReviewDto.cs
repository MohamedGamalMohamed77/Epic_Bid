using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Abstraction.Models.ProductDt
{
    public class ReviewDto
    {
        public int ProductId { get; set; } // foreign key
        public string UserId { get; set; } = string.Empty; 
        public string UserName { get; set; } = string.Empty;// name of the user who wrote the review
        public string ReviewText { get; set; } = string.Empty; // review text
        public int Rating { get; set; } // rating out of 5
        public DateTime CreatedAt { get; set; } 
    }
}
