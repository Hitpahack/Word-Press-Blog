using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpSimpleHistory
{
    public long Id { get; set; }

    public DateTime Date { get; set; }

    public string? Logger { get; set; }

    public string? Level { get; set; }

    public string? Message { get; set; }

    public string? OccasionsId { get; set; }

    public string? Initiator { get; set; }
}
