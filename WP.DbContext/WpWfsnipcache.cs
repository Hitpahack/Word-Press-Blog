using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpWfsnipcache
{
    public uint Id { get; set; }

    public string Ip { get; set; } = null!;

    public DateTime Expiration { get; set; }

    public string Body { get; set; } = null!;

    public uint Count { get; set; }

    public uint Type { get; set; }
}
