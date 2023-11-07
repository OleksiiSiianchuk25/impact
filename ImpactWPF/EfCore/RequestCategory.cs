using System;
using System.Collections.Generic;

namespace EfCore;

public partial class RequestCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
