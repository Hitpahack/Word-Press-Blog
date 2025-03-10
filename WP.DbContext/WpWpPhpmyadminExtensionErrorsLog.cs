using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpWpPhpmyadminExtensionErrorsLog
{
    public int Id { get; set; }

    public DateTime? Gmdate { get; set; }

    public string FunctionName { get; set; } = null!;

    public string FunctionArgs { get; set; } = null!;

    public string Message { get; set; } = null!;
}
