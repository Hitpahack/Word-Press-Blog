using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpWfwaffailure
{
    public uint Id { get; set; }

    public string Throwable { get; set; } = null!;

    public uint? RuleId { get; set; }

    public DateTime Timestamp { get; set; }
}
