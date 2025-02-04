using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Helpers;
using BackOffice.Models.DTOs.Employees;

namespace BackOffice.ViewModels.Employees
{
    public class EmployeeSelectorViewModel : BaseListViewModel<EmployeeSelectorDto>
    {
        public EmployeeSelectorViewModel() : base("Employees",
            "EmployeeSelector")
        {

        }
    }
}
