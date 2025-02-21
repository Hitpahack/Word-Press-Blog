using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpWfauditevent
{
    public ulong Id { get; set; }

    public string Type { get; set; } = null!;

    public string Data { get; set; } = null!;

    public double EventTime { get; set; }

    public ulong RequestId { get; set; }

    public string State { get; set; } = null!;

    public DateTime StateTimestamp { get; set; }
}
