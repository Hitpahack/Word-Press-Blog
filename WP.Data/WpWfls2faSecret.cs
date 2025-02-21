using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpWfls2faSecret
{
    public uint Id { get; set; }

    public ulong UserId { get; set; }

    public byte[] Secret { get; set; } = null!;

    public byte[] Recovery { get; set; } = null!;

    public uint Ctime { get; set; }

    public uint Vtime { get; set; }

    public string Mode { get; set; } = null!;
}
