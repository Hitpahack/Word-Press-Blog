using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpWffilemod
{
    public byte[] FilenameMd5 { get; set; } = null!;

    public string Filename { get; set; } = null!;

    public string RealPath { get; set; } = null!;

    public byte KnownFile { get; set; }

    public byte[] OldMd5 { get; set; } = null!;

    public byte[] NewMd5 { get; set; } = null!;

    public byte[] Shac { get; set; } = null!;

    public string StoppedOnSignature { get; set; } = null!;

    public uint StoppedOnPosition { get; set; }

    public string IsSafeFile { get; set; } = null!;
}
