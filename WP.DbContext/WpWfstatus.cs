using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpWfstatus
{
    public ulong Id { get; set; }

    public double Ctime { get; set; }

    public byte Level { get; set; }

    public string Type { get; set; } = null!;

    public string Msg { get; set; } = null!;
}
