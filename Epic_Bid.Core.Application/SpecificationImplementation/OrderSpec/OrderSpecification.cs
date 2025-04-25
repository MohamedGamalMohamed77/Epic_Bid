using Epic_Bid.Core.Application.SpecificationImplementation;
using Epic_Bid.Core.Domain.Entities.Order;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.SpecificationImplementation.OrderSpec
{
    public class OrderSpecification: BaseSpecification<Order>
    {
        public OrderSpecification(string buyeremail) :base(O => O.BuyerEmail == buyeremail)
        {
            Includes.Add(x => x.DeliveryMethod);
            Includes.Add(x => x.Items);
            
        }
        public OrderSpecification(string buyeremail, int orderid):base(p =>
        (p.BuyerEmail == buyeremail )
        && 
        (p.Id == orderid))
        {
            Includes.Add(x => x.DeliveryMethod);
            Includes.Add(x => x.Items);
        }
    }
}
