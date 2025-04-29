using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Domain.Entities.Basket
{
	public class CustomerBasket
	{
        public required string Id { get; set; }
		public ICollection<BasketItem> Items { get; set; } = new HashSet<BasketItem>();
	
	}
}
