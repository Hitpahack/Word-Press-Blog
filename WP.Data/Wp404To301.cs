using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class Wp404To301
{
    public long Id { get; set; }

    public DateTime Date { get; set; }

    public string Url { get; set; } = null!;

    public string Ref { get; set; } = null!;

    public string Ip { get; set; } = null!;

    public string Ua { get; set; } = null!;

    public string? Redirect { get; set; }

    public string? Options { get; set; }

    public long Status { get; set; }
}
