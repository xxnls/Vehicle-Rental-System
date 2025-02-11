using API.Interfaces;
using System;
using System.Collections.Generic;

namespace API.Models.FileSystem;

public partial class DocumentCategory : IBaseModel
{
    public int DocumentCategoryId { get; set; }

    public int? ParentCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<DocumentCategory> InverseParentCategory { get; set; } = new List<DocumentCategory>();

    public virtual DocumentCategory? ParentCategory { get; set; }
}
