using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Epic_Bid.Infrastructure.Persistence._Identity
{
	public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StoreIdentityDbContext>
	{
		public StoreIdentityDbContext CreateDbContext(string[] args)
		{
			// Create options for the DbContext
			var optionsBuilder = new DbContextOptionsBuilder<StoreIdentityDbContext>();

			// Set the connection string (can also be configured in appsettings.json)
			var connectionString = "Server=.;Database=Epic_Bid;Trusted_Connection=True;TrustServerCertificate=True;";

			// Use SQL Server for the DbContext
			optionsBuilder.UseSqlServer(connectionString);

			// Return a new instance of the DbContext with the provided options
			return new StoreIdentityDbContext(optionsBuilder.Options);
		}
	}
}