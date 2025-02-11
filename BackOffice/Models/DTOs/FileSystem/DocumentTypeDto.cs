using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.DTOs.FileSystem
{
    public class DocumentTypeDto : BaseDtoModel
    {
        public int DocumentTypeId { get; set; }

        private string _name;
        private string _description;
        private string _fileExtension;
        private int _maxFileSizeMb;

        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public string Description { get => _description; set { _description = value; OnPropertyChanged(); } }
        public string FileExtension { get => _fileExtension; set { _fileExtension = value; OnPropertyChanged(); } }
        public int MaxFileSizeMb { get => _maxFileSizeMb; set { _maxFileSizeMb = value; OnPropertyChanged(); } }
    }
}
