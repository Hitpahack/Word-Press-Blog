using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpWfblockediplog
{
    public byte[] Ip { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public uint BlockCount { get; set; }

    public uint Unixday { get; set; }

    public string BlockType { get; set; } = null!;
}
