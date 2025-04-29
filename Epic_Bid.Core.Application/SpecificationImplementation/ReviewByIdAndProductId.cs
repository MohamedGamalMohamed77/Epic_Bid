using Epic_Bid.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.SpecificationImplementation
{
    public class ReviewByIdAndProductId : BaseSpecification<CustomerReview>
    {
        public ReviewByIdAndProductId( int ProductId) : base( x => x.ProductId == ProductId)
        {
            
        }
        
    }
}
