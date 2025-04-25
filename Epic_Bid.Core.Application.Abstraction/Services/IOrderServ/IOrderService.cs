using Epic_Bid.Core.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Abstraction.Services.IOrderServ
{
    public interface IOrderService 
    {
        Task<Order> CreateOrderAsync(string BuyerEmail, string BaskedId,int DelvieryMethodId,Address ShippingAddress );
        Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail);
        Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int OrderId);


    }
}
