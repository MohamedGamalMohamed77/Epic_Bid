using Epic_Bid.Core.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Domain.Specifications
{
    public interface ISpecification<TEntity> where TEntity : BaseEntity
    {
        // Expression To Filter
        public Expression<Func<TEntity, bool>> Criteria { get; }
        // Include Related Data
        public List<Expression<Func<TEntity, object>>> Includes { get; }
        public int Take { get;  }
        public int Skip { get; }
        public bool IsPaginated { get; set; }

    }
}
