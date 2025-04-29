using Epic_Bid.Core.Application.Abstraction.Services;
using Epic_Bid.Core.Application.Abstraction.Services.AuctionServ;
using Epic_Bid.Core.Application.Abstraction.Services.Auth;
using Epic_Bid.Core.Application.Abstraction.Services.Basket;
using Epic_Bid.Core.Application.Abstraction.Services.IOrderServ;
using Epic_Bid.Core.Application.Abstraction.Services.IProductServ;
using Epic_Bid.Core.Application.Mapping;
using Epic_Bid.Core.Application.Services;
using Epic_Bid.Core.Application.Services.AuctionServ;
using Epic_Bid.Core.Application.Services.Auth;
using Epic_Bid.Core.Application.Services.Basket;
using Epic_Bid.Core.Application.Services.OrderServ;
using Epic_Bid.Core.Application.Services.ProductServ;
using Epic_Bid.Core.Domain.Contracts.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(MappingProfile));




            //services.AddScoped(typeof(IBasketService), typeof(BasketService));
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped(typeof(Func<IBasketService>), (serviceProvider) =>
			{
				return () => serviceProvider.GetRequiredService<IBasketService>();
			});

			services.AddScoped(typeof(IServiceManager), typeof(ServiceManager));
			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IAuctionService, AuctionService>();
            services.AddScoped<IProductService, ProductService>();
            return services;
		}

	}
}
