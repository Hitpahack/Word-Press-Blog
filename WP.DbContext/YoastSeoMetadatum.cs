using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class YoastSeoMetadatum
{
    public int Id { get; set; }

    public ulong Postid { get; set; }

    public string? SeoKeyphrase { get; set; }

    public string? SeoTitle { get; set; }

    public string? SeoMetatag { get; set; }

    public string? SeoSlug { get; set; }
}
