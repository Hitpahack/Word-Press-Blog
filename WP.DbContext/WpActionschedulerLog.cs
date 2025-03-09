using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpActionschedulerLog
{
    public ulong LogId { get; set; }

    public ulong ActionId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime? LogDateGmt { get; set; }

    public DateTime? LogDateLocal { get; set; }
}
