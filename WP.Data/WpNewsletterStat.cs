using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpNewsletterStat
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int EmailId { get; set; }

    public int LinkId { get; set; }

    public DateTime Created { get; set; }

    public string Url { get; set; } = null!;

    public string Anchor { get; set; } = null!;

    public string Ip { get; set; } = null!;

    public string Country { get; set; } = null!;
}
