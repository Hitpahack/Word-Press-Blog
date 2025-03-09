using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpWfnotification
{
    public string Id { get; set; } = null!;

    public byte New { get; set; }

    public string Category { get; set; } = null!;

    public int Priority { get; set; }

    public uint Ctime { get; set; }

    public string Html { get; set; } = null!;

    public string Links { get; set; } = null!;
}
