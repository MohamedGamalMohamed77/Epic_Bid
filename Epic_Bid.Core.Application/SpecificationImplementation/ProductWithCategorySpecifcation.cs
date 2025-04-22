using Epic_Bid.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.SpecificationImplementation
{
    public class ProductWithCategorySpecifcation : BaseSpecification<Product>
    {
        // For Criteria
        public ProductWithCategorySpecifcation(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductCategory);
        }

        // include
        public ProductWithCategorySpecifcation() : base(null!)
        {
            AddInclude(x => x.ProductCategory);
        }

    }
}
