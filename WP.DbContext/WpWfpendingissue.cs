using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpWfpendingissue
{
    public uint Id { get; set; }

    public uint Time { get; set; }

    public uint LastUpdated { get; set; }

    public string Status { get; set; } = null!;

    public string Type { get; set; } = null!;

    public byte Severity { get; set; }

    public string IgnoreP { get; set; } = null!;

    public string IgnoreC { get; set; } = null!;

    public string ShortMsg { get; set; } = null!;

    public string? LongMsg { get; set; }

    public string? Data { get; set; }
}
