using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpWpcAccesslock
{
    public long AccesslockId { get; set; }

    public long UserId { get; set; }

    public DateTime AccesslockDate { get; set; }

    public DateTime ReleaseDate { get; set; }

    public string AccesslockIp { get; set; } = null!;

    public string? Reason { get; set; }

    public short Unlocked { get; set; }
}
