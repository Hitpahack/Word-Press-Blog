using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpEasywpsmtpTasksMetum
{
    public long Id { get; set; }

    public string Action { get; set; } = null!;

    public string Data { get; set; } = null!;

    public DateTime Date { get; set; }
}
