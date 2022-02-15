using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miritush.DTO
{
    public class ListResult<T>
    {
        public ListResult(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public IEnumerable<T> Data { get; set; }
        public int TotalRecord { get; set; }
        public int TotalPages
        {
            get
            {
                var totalPages = ((double)this.TotalRecord / (double)this.PageSize);
                return Convert.ToInt32(Math.Ceiling(totalPages));
            }
            set { }
        }
        public int PageSize { get; }
        public int PageNumber { get; }
    }
}