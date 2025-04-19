using Epic_Bid.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Domain.Contracts.Persistence
{
    public interface IUnitOfWork: IAsyncDisposable
    {
        IGenericRepository<TEntity> GetRepository<TEntity>()where TEntity : BaseEntity;
        Task SaveChangesAsync();

    }
}
