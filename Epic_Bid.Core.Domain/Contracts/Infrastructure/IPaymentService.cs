using Epic_Bid.Core.Domain.Entities.Basket;
using Epic_Bid.Shared.Models.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Domain.Contracts.Infrastructure
{
	public interface IPaymentService
	{
		Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId);
		Task UpdateOrederPaymentStatus(string requestBody,string header);
	}
}
