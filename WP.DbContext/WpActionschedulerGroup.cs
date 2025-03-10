using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpActionschedulerGroup
{
    public ulong GroupId { get; set; }

    public string Slug { get; set; } = null!;
}
