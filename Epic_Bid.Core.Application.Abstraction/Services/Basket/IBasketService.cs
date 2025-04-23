using Epic_Bid.Shared.Models.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Abstraction.Services.Basket
{
	public interface IBasketService
	{
		Task<CustomerBasketDto> GetCustomerBasketAsync(string id);
		Task<CustomerBasketDto> UpdateCustomerBasketAsync(CustomerBasketDto basketDto);
		Task<bool> DeleteCustomerBasketAsync(string id);

	}
}
