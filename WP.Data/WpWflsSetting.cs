﻿using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpWflsSetting
{
    public string Name { get; set; } = null!;

    public byte[]? Value { get; set; }

    public string Autoload { get; set; } = null!;
}
