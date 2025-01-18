using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Interfaces
{
    public interface IBaseListViewModel
    {
        int CurrentPage { get; set; }
        int PageSize { get; set; }
        int TotalItemCount { get; set; }
        bool CanLoadNextPage { get; set; }
        bool CanLoadPreviousPage { get; set; }
        protected void UpdatePaginationState();
        protected Task LoadNextPageAsync();
        protected Task LoadPreviousPageAsync();
    }
}
