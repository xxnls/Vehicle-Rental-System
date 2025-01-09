using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Interfaces
{
    public interface IListViewModel
    {
        protected void ClearInputFields();
        protected void Cancel();
        protected void SwitchToCreateMode();
        protected void SwitchToEditMode();

    }
}
