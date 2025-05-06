using AutoMapper;
using Epic_Bid.Core.Application.SpecificationImplementation.OrderSpec;
using Epic_Bid.Core.Domain.Contracts.Infrastructure;
using Epic_Bid.Core.Domain.Contracts.Persistence;
using Epic_Bid.Core.Domain.Entities.Basket;
using Epic_Bid.Core.Domain.Entities.Order;
using Epic_Bid.Shared.Exceptions;
using Epic_Bid.Shared.Models;
using Epic_Bid.Shared.Models.Basket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.V2;
using Product = Epic_Bid.Core.Domain.Entities.Products.Product;
namespace Epic_Bid.Infrastructure.Payment_Service
{

	public class PaymentService(IBasketRepository basketRepository,
	IUnitOfWork unitOfWork,
	IMapper mapper,
	IOptions<RedisSettings> redisSettings,
	IOptions<StripeSettings> stripeSettings,
	ILogger<PaymentService> logger
	) : IPaymentService
	{
		private readonly RedisSettings _redisSettings = redisSettings.Value;
		private readonly StripeSettings _stripeSettings = stripeSettings.Value;
		public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId)
		{
			StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

			var basket = await basketRepository.GetAsync(basketId);
			if (basket is null) throw new NotFoundException(nameof(CustomerBasket), basketId);

			if (basket.DeliveryMethodId.HasValue)
			{
				var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
				if (deliveryMethod is null) throw new NotFoundException(nameof(DeliveryMethod), basket.DeliveryMethodId.Value);
				basket.ShippingPrice = deliveryMethod.Cost;

			}

			if (basket.Items.Count() > 0)
			{
				var productRepo = unitOfWork.GetRepository<Product>();
				foreach (var item in basket.Items)
				{
					var product = await productRepo.GetByIdAsync(item.Id);

					if (product is null) throw new NotFoundException(nameof(Product), item.Id);

					if (item.Price != product.Price)
						item.Price = product.Price;
				}

			}

			PaymentIntent? paymentIntent = null;
			PaymentIntentService paymentIntentService = new PaymentIntentService();
			if (string.IsNullOrEmpty(basket.PaymentIntentId))
			{
				var options = new PaymentIntentCreateOptions()
				{
					Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)basket.ShippingPrice * 100,
					Currency = "USD",
					PaymentMethodTypes = new List<string>() { "card" }

				};
				paymentIntent = await paymentIntentService.CreateAsync(options);
				basket.PaymentIntentId = paymentIntent.Id;
				basket.ClientSecret = paymentIntent.ClientSecret;


			}
			else
			{
				var options = new PaymentIntentUpdateOptions()
				{
					Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)basket.ShippingPrice * 100,
				};
				await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);

			}

			await basketRepository.UpdateAsync(basket, TimeSpan.FromDays(_redisSettings.TimeToLiveInDays));
			return mapper.Map<CustomerBasketDto>(basket);
		}

		public async Task UpdateOrederPaymentStatus(string requestBody, string header)
		{

			var stripeEvent = EventUtility.ConstructEvent(requestBody, header, _stripeSettings.webhooksecret);

			var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
			Order? order;
			switch (stripeEvent.Type)
			{
				case "payment_intent.succeeded":
					order = await UpdatePaymentIntent(paymentIntent.Id, true);
					logger.LogInformation("order is succeeded with payment intent id {0} ", paymentIntent.Id);
					break;

				case "payment_intent.payment_failed":
					order = await UpdatePaymentIntent(paymentIntent.Id, false);
					logger.LogInformation("order is failed with payment intent id {0} ", paymentIntent.Id);
					break;


			}
		}
		private async Task<Order> UpdatePaymentIntent(string paymentIntentId, bool ispaid)
		{
			var orderRepo = unitOfWork.GetRepository<Order>();

			var spec = new OrderPaymentIdSpec(paymentIntentId);

			var order = await orderRepo.GetByIdAsync(spec);
			if (order is null) throw new NotFoundException(nameof(order), $"paymentIntentId : {paymentIntentId}");
			if (ispaid)
				order.Status = OrderStatus.PaymentReceived;
			else
				order.Status = OrderStatus.PaimentFailed;

			orderRepo.Update(order);

			await unitOfWork.SaveChangesAsync();

			return order;
		}

	}

}
