using API.Interfaces;

namespace API.Models.DTOs.FileSystem
{
    public class DocumentTypeDto : IBaseModel
    {
        public int DocumentTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileExtension { get; set; }
        public int MaxFileSizeMb { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
