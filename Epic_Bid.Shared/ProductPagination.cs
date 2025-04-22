using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Shared
{
    public class ProductPagination<TEntity>
    {
        // ctor
        public ProductPagination(int pageSize, int pageIndex, int count, IReadOnlyList<TEntity> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            Data = data;
        }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<TEntity> Data { get; set; }

    }
}
