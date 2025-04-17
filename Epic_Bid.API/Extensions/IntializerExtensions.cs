using Epic_Bid.Core.Domain.Contracts.Persistence;
using Epic_Bid.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace Epic_Bid.API.Extensions
{
	public static class  IntializerExtensions
	{
		public static async Task<WebApplication> InitializeAsync(this WebApplication app)
		{ 

			using var scope = app.Services.CreateAsyncScope();
			var  services = scope.ServiceProvider;

			var storeIdentityIntializer = services.GetRequiredService<IStoreIdentityDbIntializer>() ;

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();
			try
			{

			  await	storeIdentityIntializer.InitializeAsync();
			  await	storeIdentityIntializer.SeedAsync();
			
			
			}
			catch (Exception ex) 
			{
				var logger = loggerFactory.CreateLogger<Program>();

				logger.LogError(ex, "an error has been occured during applaying migrations");

			}
			return app;

		
		}

	}
}
