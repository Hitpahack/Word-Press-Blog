using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpWfreversecache
{
    public byte[] Ip { get; set; } = null!;

    public string Host { get; set; } = null!;

    public uint LastUpdate { get; set; }
}
