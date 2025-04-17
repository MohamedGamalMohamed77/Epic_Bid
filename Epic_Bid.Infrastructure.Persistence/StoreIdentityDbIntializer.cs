using Epic_Bid.Core.Domain.Contracts.Persistence;
using Epic_Bid.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Infrastructure.Persistence
{
	public  class StoreIdentityDbIntializer(StoreIdentityDbContext _dbcontext,UserManager<ApplicationUser> _userManager) : IStoreIdentityDbIntializer
	{
		public async Task InitializeAsync()
		{
			var pendingMigrations =await _dbcontext.Database.GetPendingMigrationsAsync();

			if (pendingMigrations.Any())
				await _dbcontext.Database.MigrateAsync();
			
		}

		public async Task SeedAsync()
		{
			var user = new ApplicationUser()
			{ 
			DisplayName="Mohamed Gamal",
			Email="mg1791@fayoum.edu.eg",
			UserName= "mg1791@fayoum.edu.eg",
			PhoneNumber="01024226225"
			};
			await _userManager.CreateAsync(user,"P@ssw0rd");
		}
	}
}
