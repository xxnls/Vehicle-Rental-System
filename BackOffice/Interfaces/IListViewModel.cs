using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Interfaces
{
    public interface IListViewModel
    {
        protected Task CreateModelAsync();
        protected Task UpdateModelAsync();
    }
}
