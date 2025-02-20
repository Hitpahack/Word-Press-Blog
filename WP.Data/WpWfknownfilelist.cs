using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpWfknownfilelist
{
    public uint Id { get; set; }

    public string Path { get; set; } = null!;

    public string WordpressPath { get; set; } = null!;
}
