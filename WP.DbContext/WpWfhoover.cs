using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpWfhoover
{
    public uint Id { get; set; }

    public string? Owner { get; set; }

    public string? Host { get; set; }

    public string? Path { get; set; }

    public byte[]? HostKey { get; set; }
}
