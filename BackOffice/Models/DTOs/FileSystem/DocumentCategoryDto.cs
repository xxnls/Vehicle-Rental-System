using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.DTOs.FileSystem
{
    public class DocumentCategoryDto : BaseDtoModel
    {
        public int DocumentCategoryId { get; set; }

        private int? _parentCategoryId;
        private string _name;
        private string _description;

        public int? ParentCategoryId { get => _parentCategoryId; set { _parentCategoryId = value; OnPropertyChanged(); } }
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public string Description { get => _description; set { _description = value; OnPropertyChanged(); } }
    }
}
