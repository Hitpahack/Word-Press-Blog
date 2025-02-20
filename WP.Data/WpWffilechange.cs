using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpWffilechange
{
    public string FilenameHash { get; set; } = null!;

    public string File { get; set; } = null!;

    public string Md5 { get; set; } = null!;
}
