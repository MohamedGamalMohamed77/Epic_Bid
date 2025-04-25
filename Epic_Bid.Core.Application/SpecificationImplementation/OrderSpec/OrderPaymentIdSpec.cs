using Epic_Bid.Core.Application.SpecificationImplementation;
using Epic_Bid.Core.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Epic_Bid.Core.Application.SpecificationImplementation.OrderSpec
{
    public class OrderPaymentIdSpec : BaseSpecification<Order>
    {
        public OrderPaymentIdSpec(string PaymentIntendId) : base(x => x.PaymentIntentId == PaymentIntendId)
        {
        }
    }
}
