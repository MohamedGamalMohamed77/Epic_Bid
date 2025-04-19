using Epic_Bid.Core.Domain.Common;
using Epic_Bid.Core.Domain.Contracts.Persistence;
using Epic_Bid.Infrastructure.Persistence._Identity.Config;
using Epic_Bid.Infrastructure.Persistence.Generic_Reposetories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private  Hashtable Repositroy;
        public StoreIdentityDbContext _dbContext { get; }
        public UnitOfWork(StoreIdentityDbContext DbContext)
        {
            this._dbContext = DbContext;
            Repositroy = new Hashtable();
        }



        public ValueTask DisposeAsync()
        => _dbContext.DisposeAsync();

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            // Check if the repository already exists in the hashtable
            if (Repositroy.ContainsKey(typeof(TEntity)))
            {
                return (IGenericRepository<TEntity>)Repositroy[typeof(TEntity)];
            }
            else
            {
                //Create type
                var type = typeof(TEntity);
                if (!Repositroy.ContainsKey(type.Name))
                {
                    var repository = new GenericRepository<TEntity>(_dbContext);
                    Repositroy.Add(type.Name, repository);
                }
                return Repositroy[type.Name] as IGenericRepository<TEntity>;

            }
        }

        public async Task SaveChangesAsync()
        => await _dbContext.SaveChangesAsync();
    }
}
