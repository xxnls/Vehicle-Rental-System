using API.Interfaces;
using Microsoft.Identity.Client;

namespace API.Models.DTOs.FileSystem
{
    public class DocumentCategoryDto : IBaseModel
    {
        public int DocumentCategoryId { get; set; }
        public int? ParentCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsActive { get; set; }

        public DocumentCategoryDto? ParentCategory { get; set; }
    }
}
