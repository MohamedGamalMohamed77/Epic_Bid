using Epic_Bid.Core.Domain.Entities.Products;
using Epic_Bid.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.SpecificationImplementation
{
    public class ProductFilteration:BaseSpecification<Product>
    {
        public ProductFilteration(ProductParamQuery? param) :base(
            (x => (string.IsNullOrEmpty(param.Category) || x.ProductCategory.Name.ToLower().Contains(param.Category.ToLower()))
            && (param.StartPrice == null || (x.Price >= param.StartPrice && x.Price <= param.EndPrice))
            && (string.IsNullOrEmpty(param.Color) || x.Color.ToLower().Contains(param.Color.ToLower()))
            && (string.IsNullOrEmpty(param.SearchValue)|| x.Name.ToLower().Contains(param.SearchValue)))
            )
        {
            ApplyPaging(param.PageSize , param.PageIndex);

        }
    }
}
