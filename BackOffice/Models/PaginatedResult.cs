using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalItemCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
