using Epic_Bid.Core.Domain.Entities.Products;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Epic_Bid.Core.Domain.Entities.Roles;
using Epic_Bid.Core.Domain.Entities.Auth;
using Epic_Bid.Core.Domain.Entities.Order;

namespace Epic_Bid.Infrastructure.Persistence
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
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<AuctionBid> AuctionBids { get; set; } // المزادات

    }
}
