using Epic_Bid.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Domain.Entities.Products
{
    public class ProductCategory: BaseEntity
    {
        public string Name { get; set; }

        // navigational property
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
