using Epic_Bid.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Domain.Entities.Products
{
    public class Product : BaseEntity
    {
        public string UserCreatedId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty; // اسم المنتج
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; } // السعر قبل الخصم (لو فيه خصم)
        public bool InStock { get; set; } = true;

       
        //public double? AverageRating { get; set; }
        //public int? TotalRatings { get; set; }
        //public int? UnitsSold { get; set; }

        public string Color { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Dimensions { get; set; } = string.Empty; // 24×24.4×35.8
        //public string Brand { get; set; } = string.Empty;
        //public string Category { get; set; } = string.Empty;

        // navigation properties Of Category 
        public int ProductCategoryId { get; set; } // foreign key
        public ProductCategory ProductCategory { get; set; } = null!;
       
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<CustomerReview>? Reviews { get; set; } = new List<CustomerReview>();
    }
    
}
