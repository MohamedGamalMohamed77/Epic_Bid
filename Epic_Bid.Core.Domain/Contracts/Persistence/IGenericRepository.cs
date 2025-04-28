using Epic_Bid.Core.Domain.Common;
using Epic_Bid.Core.Domain.Entities.Auth;
using Epic_Bid.Core.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Domain.Contracts.Persistence
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<ApplicationUser?> GetUserByIdAsycn(string id);
        #region With Specificatoin
        Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecification<TEntity> Specification);
        Task<TEntity?> GetByIdAsync(ISpecification<TEntity> Specification);
        // Get Count 
        Task<int> GetCountAsync(ISpecification<TEntity> Specification);
        #endregion

    }
}
