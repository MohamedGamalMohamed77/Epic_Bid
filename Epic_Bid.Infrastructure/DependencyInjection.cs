using Epic_Bid.Core.Domain.Contracts.Infrastructure;
using Epic_Bid.Infrastructure.Basket_Repository;
using Epic_Bid.Infrastructure.Payment_Service;
using Epic_Bid.Shared.Models;
using Epic_Bid.Shared.Models.Basket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;


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
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            

            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));
			services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));
			return services;
        }

    }
}
