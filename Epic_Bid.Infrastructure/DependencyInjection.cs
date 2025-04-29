using Epic_Bid.Core.Application.Abstraction.Services.Basket;
using Epic_Bid.Core.Application.Services.Basket;
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

///namespace Epic_Bid.Infrastructure
//{
//	public static class DependencyInjection
//	{
//		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
//		{
//            //services.AddScoped(typeof(IBasketService), typeof(BasketService));
//            services.AddScoped(typeof(IBasketService), typeof(BasketService));

//            services.AddSingleton(typeof(IConnectionMultiplexer), (serviceProvider) =>
//            {
//                var redisConfig = new ConfigurationOptions
//                {
//                    EndPoints = { "cheerful-bream-30522.upstash.io:6379" },
//                    Password = "AXc6AAIjcDEyZmY0MzY2Njc2YWI0YjExODg2MDdjOWYwMGE4OWVhZXAxMA",
//                    Ssl = true,
//                    AbortOnConnectFail = false,
//                    ConnectTimeout = 10000 // optional: زيادة المهلة
//                };

//                return ConnectionMultiplexer.Connect(redisConfig);
//            });


//            return services;
//		}

//	}
//}
namespace Epic_Bid.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(typeof(IConnectionMultiplexer), (serviceProvider) =>
            {
                try
                {
                    var connectionString = configuration.GetConnectionString("Redis")!;

                    ConfigurationOptions config;

                    if (connectionString.StartsWith("redis://") || connectionString.StartsWith("rediss://"))
                    {
                        // ✅ Handle Upstash (cloud-hosted Redis)
                        var uri = new Uri(connectionString);
                        var userInfo = uri.UserInfo.Split(':');

                        config = new ConfigurationOptions
                        {
                            EndPoints = { { uri.Host, uri.Port } },
                            Ssl = uri.Scheme == "rediss",           // Ssl only for rediss
                            AbortOnConnectFail = false,
                            User = userInfo[0],
                            Password = userInfo[1],
                        };
                    }
                    else
                    {
                        // ✅ Handle localhost or custom Redis server
                        config = ConfigurationOptions.Parse(connectionString);
                        config.AbortOnConnectFail = false;
                    }
                    var connectionMultiplexer = ConnectionMultiplexer.Connect(config);
                    return connectionMultiplexer;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Redis connection error: " + ex.Message);
                    throw;
                }

            });


            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));

            return services;
        }

    }
}