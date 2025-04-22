using Epic_Bid.Core.Domain.Contracts.Persistence;
using Epic_Bid.Core.Domain.Entities;
using Epic_Bid.Infrastructure.Persistence._Identity.Config;
using Epic_Bid.Infrastructure.Persistence.UnitofWork;
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
			// we made this instead of we write in OnConfiguring method 
			services.AddDbContext<StoreIdentityDbContext>((options) =>
			{
				options
				
				.UseSqlServer(Configuration.GetConnectionString("StoreIdentityContext"));
			});
			
			services.AddScoped(typeof(IStoreIdentityDbIntializer), typeof(StoreIdentityDbIntializer));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            

            #endregion




            return services;
		}

	}
}
