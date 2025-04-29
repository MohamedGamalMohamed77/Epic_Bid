using Epic_Bid.Core.Domain.Entities.Order;
using System.Runtime.CompilerServices;

namespace Epic_Bid.Core.Application.Abstraction.Models.Order
{
    public class OrderToReturnDto
    {
         
        public int Id { get; set; }

        public string BuyerEmail { get; set; } = string.Empty;
        public DateTimeOffset OrderDate { get; set; } 
        public OrderStatus Status { get; set; } 
        public Address ShippingAddress { get; set; } = null!;

        public string DeliveryMethod { get; set; } = string.Empty;
        public decimal DeliveryMethodCost { get; set; }


        //Navigational Property for Many to One relationship
        public IReadOnlyList<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;


    }
}
