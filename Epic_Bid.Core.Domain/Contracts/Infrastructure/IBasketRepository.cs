using Epic_Bid.Core.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Domain.Contracts.Infrastructure
{
	public interface IBasketRepository
	{
		Task<CustomerBasket?> GetAsync(string id);

		Task<CustomerBasket?> UpdateAsync(CustomerBasket basket, TimeSpan timeSpan);

		Task<bool> DeleteAsync(string id);

	}
}
