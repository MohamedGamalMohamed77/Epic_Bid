using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Abstraction.Models.ProductDt
{
    public class CreateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile ImageUploaded { get; set; } = null!;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public bool InStock { get; set; } = true;
        public string Color { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Dimensions { get; set; } = string.Empty;
        public int ProductCategoryId { get; set; }
    }
}
