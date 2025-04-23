using Epic_Bid.Core.Domain.Contracts.Infrastructure;
using Epic_Bid.Infrastructure.Basket_Repository;
using Epic_Bid.Shared.Models.Basket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
		{
			services.AddSingleton(typeof(IConnectionMultiplexer), (serviceProvider) =>
			{
				var connectionString = configuration.GetConnectionString("Redis");
				var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString!);
				return connectionMultiplexer;
			});

			services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

			services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));

			return services;
		}

	}
}
