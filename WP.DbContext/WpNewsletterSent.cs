using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpNewsletterSent
{
    public uint EmailId { get; set; }

    public uint UserId { get; set; }

    public byte Status { get; set; }

    public byte Open { get; set; }

    public uint Time { get; set; }

    public string Error { get; set; } = null!;

    public string Ip { get; set; } = null!;
}
