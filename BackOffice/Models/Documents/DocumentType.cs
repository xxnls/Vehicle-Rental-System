using System;
using System.Collections.Generic;

namespace BackOffice.Models.Documents;

public partial class DocumentType
{
    public int DocumentTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string FileExtension { get; set; } = null!;

    public int MaxFileSizeMb { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
