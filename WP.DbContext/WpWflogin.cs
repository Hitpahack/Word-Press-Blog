using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpWflogin
{
    public uint Id { get; set; }

    public int? HitId { get; set; }

    public double Ctime { get; set; }

    public byte Fail { get; set; }

    public string Action { get; set; } = null!;

    public string Username { get; set; } = null!;

    public uint UserId { get; set; }

    public byte[]? Ip { get; set; }

    public string? Ua { get; set; }
}
