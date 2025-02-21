using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpTermmetum
{
    public ulong MetaId { get; set; }

    public ulong TermId { get; set; }

    public string? MetaKey { get; set; }

    public string? MetaValue { get; set; }
}
