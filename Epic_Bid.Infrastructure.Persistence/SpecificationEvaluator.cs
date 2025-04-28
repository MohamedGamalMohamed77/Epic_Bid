using Epic_Bid.Core.Domain.Common;
using Epic_Bid.Core.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Infrastructure.Persistence
{
    static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity>(IQueryable<TEntity> BaseQuery, ISpecification<TEntity> Specification) where TEntity : BaseEntity
        {
            var Query = BaseQuery;
            if(Specification.Criteria != null)
            {
                Query = Query.Where(Specification.Criteria);
            }
            // Sorting
            if (Specification.OrderBy != null)
            {
                Query = Query.OrderBy(Specification.OrderBy);
            }
            else if (Specification.OrderByDescending != null)
            {
                Query = Query.OrderByDescending(Specification.OrderByDescending);
            }
            // Inlcude
            if (Specification.Includes != null)
            {
                
                Query = Specification.Includes.Aggregate(Query, (current, include) => current.Include(include));
            }
            // paginated
            if (Specification.IsPaginated)
            {
                Query = Query.Skip(Specification.Skip).Take(Specification.Take);
            }
            return Query;
        }

    }
}
