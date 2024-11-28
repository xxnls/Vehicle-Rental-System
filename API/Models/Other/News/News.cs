using API.Models.Documents;
using API.Models.Employees;
using System;
using System.Collections.Generic;

namespace API.Models.Other.News;

public partial class News
{
    public int NewsId { get; set; }

    public int NewsTypeId { get; set; }

    public int? ImageId { get; set; }

    public int Content { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int CreatedByEmployeeId { get; set; }

    public int? ModifiedByEmployeeId { get; set; }

    public bool IsActive { get; set; }

    public virtual Employee CreatedByEmployee { get; set; } = null!;

    public virtual Document? Image { get; set; }

    public virtual Employee? ModifiedByEmployee { get; set; }

    public virtual NewsType NewsType { get; set; } = null!;
}
