using Epic_Bid.Core.Domain.Common;
using Epic_Bid.Core.Domain.Contracts.Persistence;
using Epic_Bid.Core.Domain.Entities.Auth;
using Epic_Bid.Core.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Identity.Client;
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


        public async Task<ApplicationUser?> GetUserByIdAsycn(string id)
        => await _Dbcontext.Set<ApplicationUser>().FirstOrDefaultAsync(i => i.Id == id);
        public void Update(TEntity entity)
        => _Dbcontext.Set<TEntity>().Update(entity);

        #region WithSpecifciation
        public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecification<TEntity> Specification)
        {
            return await SpecificationEvaluator.CreateQuery<TEntity>(_Dbcontext.Set<TEntity>(),Specification).ToListAsync();
           
        }

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity> Specification)
        {
            return await SpecificationEvaluator.CreateQuery<TEntity>(_Dbcontext.Set<TEntity>(), Specification).FirstOrDefaultAsync();
        } 
        public async Task<int>  GetCountAsync(ISpecification<TEntity> Specification)
        {
            return await SpecificationEvaluator.CreateQuery<TEntity>(_Dbcontext.Set<TEntity>(), Specification).CountAsync();
        }
        #endregion
    }
}
