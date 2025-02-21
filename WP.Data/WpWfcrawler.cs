using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpWfcrawler
{
    public byte[] Ip { get; set; } = null!;

    public byte[] PatternSig { get; set; } = null!;

    public string Status { get; set; } = null!;

    public uint LastUpdate { get; set; }

    public string? Ptr { get; set; }
}
