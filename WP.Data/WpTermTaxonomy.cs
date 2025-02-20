using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpTermTaxonomy
{
    public ulong TermTaxonomyId { get; set; }

    public ulong TermId { get; set; }

    public string Taxonomy { get; set; } = null!;

    public string Description { get; set; } = null!;

    public ulong Parent { get; set; }

    public long Count { get; set; }
}
