using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpPostmetum
{
    public ulong MetaId { get; set; }

    public ulong PostId { get; set; }

    public string? MetaKey { get; set; }

    public string? MetaValue { get; set; }
}
