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
    public class DocumentTypesViewModel : BaseListViewModel<DocumentTypeDto>, IBaseListViewModel
    {
        public DocumentTypesViewModel() : base("DocumentTypes", LocalizationHelper.GetString("Files", "DisplayNameDocumentTypes"))
        {

        }
    }
}
