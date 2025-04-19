using Epic_Bid.Core.Domain.Common;
using Epic_Bid.Core.Domain.Contracts.Persistence;
using Epic_Bid.Infrastructure.Persistence._Identity.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Infrastructure.Persistence.Generic_Reposetories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        public GenericRepository(StoreIdentityDbContext dbcontext)
        {
            _Dbcontext = dbcontext;
        }

        public StoreIdentityDbContext _Dbcontext { get; }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await _Dbcontext.Set<TEntity>().ToListAsync<TEntity>();
        }
        public async Task<TEntity?> GetByIdAsync(int id)
        => await _Dbcontext.Set<TEntity>().FindAsync(id) ;

        public async Task AddAsync(TEntity entity)
        => await _Dbcontext.Set<TEntity>().AddAsync(entity);

        public void Delete(TEntity entity)
        => _Dbcontext.Set<TEntity>().Remove(entity);



        public void Update(TEntity entity)
        => _Dbcontext.Set<TEntity>().Update(entity);
        
    }
}
