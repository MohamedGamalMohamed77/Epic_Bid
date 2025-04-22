using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Shared
{
    public class ProductParamQuery
    {
        public string? Category { get; set; } = null;
        public decimal? StartPrice { get; set; } = null;
        public decimal? EndPrice { get; set; } = null;
        public string? Color { get; set; } = null;
        public string? SearchValue { get; set; }
        private const int MaxPageSize = 10; // Default number of items per page
        private const int DefaultPageSize = 5; // Default number of items per page
        public int PageIndex { get; set; } = 1; // Current page number

        // full propery if Pagesize and validate on set
        private int pageSize = DefaultPageSize;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
    }
}
