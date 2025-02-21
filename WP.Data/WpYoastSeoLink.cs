using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpYoastSeoLink
{
    public ulong Id { get; set; }

    public string? Url { get; set; }

    public ulong? PostId { get; set; }

    public ulong? TargetPostId { get; set; }

    public string? Type { get; set; }

    public uint? IndexableId { get; set; }

    public uint? TargetIndexableId { get; set; }

    public uint? Height { get; set; }

    public uint? Width { get; set; }

    public uint? Size { get; set; }

    public string? Language { get; set; }

    public string? Region { get; set; }
}
