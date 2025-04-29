using Epic_Bid.Core.Application.Abstraction.Common;
using Epic_Bid.Core.Domain.Entities.Order;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
namespace Epic_Bid.Core.Application.Abstraction.Models.Order
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; } = string.Empty;
        [Required]
        public int DeliverMethodId { get; set; }
        [Required]
        public AddressDto ShippingAddress { get; set; } = null!;
    }
}
