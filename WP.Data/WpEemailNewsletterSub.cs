using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpEemailNewsletterSub
{
    public int EemailIdSub { get; set; }

    public string EemailNameSub { get; set; } = null!;

    public string EemailEmailSub { get; set; } = null!;

    public string EemailStatusSub { get; set; } = null!;

    public DateOnly EemailDateSub { get; set; }
}
