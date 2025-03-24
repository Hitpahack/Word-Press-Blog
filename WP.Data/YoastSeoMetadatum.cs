using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class YoastSeoMetadatum
{
    public int Id { get; set; }

    public long Postid { get; set; }

    public string? SeoKeyphrase { get; set; }

    public string? SeoTitle { get; set; }

    public string? SeoMetatag { get; set; }

    public string? SeoSlug { get; set; }
}
