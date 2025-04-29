using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Shared.Models.Basket
{
	public class CustomerBasketDto
	{
		public required string Id { get; set; }
		public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
	}
}
