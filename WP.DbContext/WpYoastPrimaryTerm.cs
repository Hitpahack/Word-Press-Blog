using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpYoastPrimaryTerm
{
    public uint Id { get; set; }

    public long? PostId { get; set; }

    public long? TermId { get; set; }

    public string Taxonomy { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public long BlogId { get; set; }
}
