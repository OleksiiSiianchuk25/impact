using System;
using System.Collections.Generic;

namespace EFCore;

public partial class RequestRole
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
