using Epic_Bid.Core.Domain.Contracts.Persistence;
using Epic_Bid.Core.Domain.Entities.Auth;
using Epic_Bid.Core.Domain.Entities.Products;
using Epic_Bid.Core.Domain.Entities.Roles;
using Epic_Bid.Infrastructure.Persistence._IdentityAndData.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Epic_Bid.Infrastructure.Persistence
{
    public class StoreIdentityDbIntializer(StoreIdentityDbContext _dbcontext, UserManager<ApplicationUser> _userManager, RoleManager<AppRole> _roleManager) : IStoreIdentityDbIntializer
	{
		public async Task InitializeAsync()
		{
			//var pendingMigrations =await _dbcontext.Database.GetPendingMigrationsAsync();

			//if (pendingMigrations.Any())
			
				await _dbcontext.Database.MigrateAsync();
			
				
		}

		public async Task SeedAsync()
		{
			var user = new ApplicationUser()
			{
				DisplayName = "Mohamed Gamal",
				Email = "mg1791@fayoum.edu.eg",
				UserName = "mg1791@fayoum.edu.eg",
				PhoneNumber = "01024226225"
			};
			await _userManager.CreateAsync(user, "P@ssw0rd");

			// Seeding the Roles 
			if (!_roleManager.Roles.Any())
			{
				var Roles = new List<AppRole>()
				{
					new AppRole() { Name = "Admin" , NormalizedName = "Admin".ToUpper(),ConcurrencyStamp = Guid.NewGuid().ToString()},
					new AppRole() { Name = "Bayer" ,NormalizedName = "Bayer".ToUpper(),ConcurrencyStamp = Guid.NewGuid().ToString()},
					new AppRole() { Name = "Seller" ,NormalizedName = "Seller".ToUpper(),ConcurrencyStamp = Guid.NewGuid().ToString()}
				};
				foreach (var Role in Roles)
				{
					await _roleManager.CreateAsync(Role);
				}
			}


			// StoreContextInitializer seeding
			if (!_dbcontext.ProductCategories.Any())
			{
				// Reading the data form json file
				var Data = File.ReadAllText("../Epic_Bid.Infrastructure.Persistence/_IdentityAndData/DataSeed/ProductCategories.json");
				// Deserializing the data
				var ProductCategories = JsonSerializer.Deserialize<List<ProductCategory>>(Data);
				// Adding the data to the database
				if (ProductCategories?.Count > 0)
				{
					foreach (var item in ProductCategories)
					{
						await _dbcontext.ProductCategories.AddAsync(item);
					}
					await _dbcontext.SaveChangesAsync();
				}
			}

			if (!_dbcontext.Products.Any())
			{
				// Reading the data form json file
				var Data = File.ReadAllText("../Epic_Bid.Infrastructure.Persistence/_IdentityAndData/DataSeed/Product.json");
				// Deserializing the data
				var Products = JsonSerializer.Deserialize<List<Product>>(Data);
				// Adding the data to the database
				if (Products?.Count > 0)
				{
					foreach (var item in Products)
					{
						await _dbcontext.Products.AddAsync(item);
					}
					await _dbcontext.SaveChangesAsync();
				}
			}
			if (!_dbcontext.CustomersReviews.Any())
			{
				// Reading the data form json file
				var Data = File.ReadAllText("../Epic_Bid.Infrastructure.Persistence/_IdentityAndData/DataSeed/Reviews.json");
				// Deserializing the data
				var CustomersReviews = JsonSerializer.Deserialize<List<CustomerReview>>(Data);
				// Adding the data to the database
				if (CustomersReviews?.Count > 0)
				{
					foreach (var item in CustomersReviews)
					{
						await _dbcontext.CustomersReviews.AddAsync(item);
					}
					await _dbcontext.SaveChangesAsync();
				}
			}
		}
	}
}
