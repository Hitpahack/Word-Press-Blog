using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpYoastIndexableHierarchy
{
    public uint IndexableId { get; set; }

    public uint AncestorId { get; set; }

    public uint? Depth { get; set; }

    public long BlogId { get; set; }
}
