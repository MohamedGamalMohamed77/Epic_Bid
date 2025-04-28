using Epic_Bid.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Abstraction.Models.ProductDt
{
    public class ProductByIdDto
    {
        public int Id { get; set; }
        public string UserCreatedId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty; // اسم المنتج
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; } // السعر قبل الخصم (لو فيه خصم)
        public bool InStock { get; set; } = true;


        public double? AverageRating { get; set; }
        public int? TotalRatings { get; set; }
        public int? UnitsSold { get; set; }

        public string Color { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Dimensions { get; set; } = string.Empty; // 24×24.4×35.8

        public int ProductCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty; // اسم الفئة

        public bool IsAuction { get; set; } = false; // هل المنتج معروض كمزاد؟
        public DateTime? AuctionStartTime { get; set; } // بداية المزاد
        public DateTime? AuctionEndTime { get; set; } // نهاية المزاد
        public decimal? CurrentBid { get; set; } // أعلى مزايدة حالية
        public string? CurrentWinnerUserId { get; set; } // المستخدم اللي عامل أعلى مزايدة
        public bool IsAuctionClosed { get; set; } = false; // هل المزاد انتهى؟ 
        public DateTime CreatedAt { get; set; }

    }
}
