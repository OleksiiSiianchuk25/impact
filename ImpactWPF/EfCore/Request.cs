using System;
using System.Collections.Generic;

namespace EfCore;

public partial class Request
{
    public int RequestId { get; set; }

    public string RequestName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;

    public int? CreatorUserRef { get; set; }

    public string ContactPhone { get; set; } = null!;

    public string ContactEmail { get; set; } = null!;

    public int RoleRef { get; set; }

    public virtual User? CreatorUserRefNavigation { get; set; }

    public virtual RequestRole RoleRefNavigation { get; set; } = null!;

    public virtual ICollection<RequestCategory> Categories { get; set; } = new List<RequestCategory>();
}
