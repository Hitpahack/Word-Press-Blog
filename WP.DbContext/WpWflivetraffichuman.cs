﻿using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpWflivetraffichuman
{
    public byte[] Ip { get; set; } = null!;

    public byte[] Identifier { get; set; } = null!;

    public uint Expiration { get; set; }
}
