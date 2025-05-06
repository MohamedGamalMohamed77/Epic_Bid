using AutoMapper;
using Epic_Bid.Core.Application.Abstraction.Services.Basket;
using Epic_Bid.Shared.Exceptions;
using Epic_Bid.Core.Domain.Contracts.Infrastructure;
using Epic_Bid.Core.Domain.Entities.Basket;
using Epic_Bid.Shared.Models.Basket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Services.Basket
{
	public class BasketService(IBasketRepository basketRepository,IConfiguration configuration,IMapper mapper) : IBasketService
	{
		public async Task<CustomerBasketDto> GetCustomerBasketAsync(string basketId)
		{
			var basket = await basketRepository.GetAsync(basketId);
			if (basket is null) throw new NotFoundException(nameof(CustomerBasket), basketId);
			return mapper.Map<CustomerBasketDto>(basket);
		}
		public async Task<CustomerBasketDto> UpdateCustomerBasketAsync(CustomerBasketDto basketDto)
		{
			var basket = mapper.Map<CustomerBasket>(basketDto);

			var timeToLive = TimeSpan.FromDays(double.Parse(configuration.GetSection("RedisSettings")["TimeToLiveInDays"]!));

			var updatedBasket = await basketRepository.UpdateAsync(basket, timeToLive);

			if (updatedBasket is null) throw new BadRequestException("can't update,there is a problem with your basket ");

			return basketDto;
		}
		public async Task<bool> DeleteCustomerBasketAsync(string basketId)
		{
			var deleted = await basketRepository.DeleteAsync(basketId);
			if (!deleted) throw new BadRequestException("unable to delete this basket");
			return deleted;
		}
	}
}
