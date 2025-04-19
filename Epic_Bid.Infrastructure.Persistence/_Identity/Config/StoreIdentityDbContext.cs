using Epic_Bid.Core.Domain.Entities;
using Epic_Bid.Core.Domain.Entities.Products;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Epic_Bid.Core.Domain.Entities.Roles;

namespace Epic_Bid.Infrastructure.Persistence._Identity.Config
{
    public class StoreIdentityDbContext : IdentityDbContext<ApplicationUser, AppRole, string>
    {
        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // it call all classes use ientitytypeconfiguration interface  
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<CustomerReview> CustomersReviews { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
    }
}
