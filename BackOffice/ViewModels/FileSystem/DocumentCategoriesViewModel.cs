using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models.DTOs.FileSystem;

namespace BackOffice.ViewModels.FileSystem
{
    public class DocumentCategoriesViewModel : BaseListViewModel<DocumentCategoryDto>, IBaseListViewModel
    {
        public DocumentCategoriesViewModel() : base("DocumentCategories", LocalizationHelper.GetString("Files", "DisplayNameDocumentCategories"))
        {
        }
    }
}
