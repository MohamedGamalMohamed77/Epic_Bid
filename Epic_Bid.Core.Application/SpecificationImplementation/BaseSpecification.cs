using Epic_Bid.Core.Domain.Common;
using Epic_Bid.Core.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.SpecificationImplementation
{
    public abstract class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : BaseEntity
    {
        protected BaseSpecification(Expression<Func<TEntity,bool>> CriterriaExpression)
        {
            Criteria = CriterriaExpression;
        }
        public Expression<Func<TEntity, bool>> Criteria { get; private set; }
        #region Include
        public List<Expression<Func<TEntity, object>>> Includes { get; } = [];

        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        } 
        #endregion

        #region Paginated
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPaginated { get ; set; }

        protected void ApplyPaging(int pageSize, int pageIndex)
        {
            IsPaginated = true;
            Skip = (pageIndex - 1) * pageSize;
            Take = pageSize;
        }
        #endregion

    }
}
