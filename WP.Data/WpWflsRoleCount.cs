using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpWflsRoleCount
{
    public byte[] SerializedRoles { get; set; } = null!;

    public bool TwoFactorInactive { get; set; }

    public ulong UserCount { get; set; }
}
