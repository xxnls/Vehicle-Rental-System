using BackOffice.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BackOffice.Models
{
    /// <summary>
    /// Parameters for the selector dialog.
    /// </summary>
    public class SelectorDialogParameters
    {
        public Type SelectorViewModelType { get; set; }
        public UserControl SelectorView { get; set; }   
        public Action<object> TargetProperty { get; set; }
        public string PropertyForSelection { get; set; }
        public string Title { get; set; }
    }
}
