﻿using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpWfconfig
{
    public string Name { get; set; } = null!;

    public byte[]? Val { get; set; }

    public string Autoload { get; set; } = null!;
}
