using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpEemailNewsletter
{
    public int EemailId { get; set; }

    public string EemailSubject { get; set; } = null!;

    public string EemailContent { get; set; } = null!;

    public string EemailStatus { get; set; } = null!;

    public DateTime EemailDate { get; set; }
}
