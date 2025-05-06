using Epic_Bid.Apis.Controllers.Controllers.Base;
using Epic_Bid.Core.Domain.Contracts.Infrastructure;
using Epic_Bid.Shared.Models.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Apis.Controllers.Controllers.Payment
{

	public class PaymentController(IPaymentService paymentService) : BaseApiController
	{
		[Authorize]
		[HttpPost("{basketId}")]
		public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var result = await paymentService.CreateOrUpdatePaymentIntent(basketId);
			return Ok(result);
		}
		[HttpPost("webhook")]
		public async Task<IActionResult> StripeWebhook()
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

			await paymentService.UpdateOrederPaymentStatus(json, Request.Headers["Stripe-Signature"]!);

			return Ok();
		}


	}
}
