using Epic_Bid.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Domain.Entities.Products
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; } // السعر قبل الخصم (لو فيه خصم)
        public bool InStock { get; set; } = true;

       
        public double? AverageRating { get; set; }
        public int? TotalRatings { get; set; }
        public int? UnitsSold { get; set; }

        public string Color { get; set; }
        public string Size { get; set; }
        public string Dimensions { get; set; } // 24×24.4×35.8
        public string Brand { get; set; }
        public string Category { get; set; }

        // navigation properties Of Category 
        public int ProductCategoryId { get; set; } // foreign key
        public ProductCategory ProductCategory { get; set; }
       
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<CustomerReview>? Reviews { get; set; }
    }
    
}
