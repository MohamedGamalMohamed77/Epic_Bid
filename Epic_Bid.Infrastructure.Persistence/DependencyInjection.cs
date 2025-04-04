using Epic_Bid.Core.Domain.Entities;
using Epic_Bid.Infrastructure.Persistence._Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Infrastructure.Persistence
{
    public static class DependencyInjection
	{
		public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration Configuration)
		{

			#region IdentityDbContext
			services.AddDbContext<StoreIdentityDbContext>((options) =>
			{
				options
				.UseLazyLoadingProxies()
				.UseSqlServer(Configuration.GetConnectionString("StoreIdentityContext"));
			});
			
			//services.AddScoped(typeof(IStoreIdentityDbIntializer), typeof(StoreIdentityDbIntializer));

			#endregion


			return services;
		}

	}
}
