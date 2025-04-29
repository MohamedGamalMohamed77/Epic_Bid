using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Domain.Contracts.Persistence
{
	public interface IStoreIdentityDbIntializer
	{
		Task InitializeAsync();

		Task SeedAsync();
	}

}
