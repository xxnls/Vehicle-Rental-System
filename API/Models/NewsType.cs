using System;
using System.Collections.Generic;

namespace API.Models;

public partial class NewsType
{
    public int NewsTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public string Color { get; set; } = null!;

    public int MaxContentSize { get; set; }

    public bool AllowImage { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();
}
