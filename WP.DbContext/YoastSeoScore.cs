using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class YoastSeoScore
{
    public ulong Id { get; set; }

    public ulong PostId { get; set; }

    public string? CatId { get; set; }

    public string PostType { get; set; } = null!;

    public string? ReadabilityScore { get; set; }

    public string? SeoScore { get; set; }
}
