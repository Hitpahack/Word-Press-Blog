using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpWftrafficrate
{
    public uint EMin { get; set; }

    public byte[] Ip { get; set; } = null!;

    public string HitType { get; set; } = null!;

    public uint Hits { get; set; }
}
