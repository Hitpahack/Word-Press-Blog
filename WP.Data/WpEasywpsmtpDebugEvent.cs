using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpEasywpsmtpDebugEvent
{
    public uint Id { get; set; }

    public string? Content { get; set; }

    public string? Initiator { get; set; }

    public byte EventType { get; set; }

    public DateTime CreatedAt { get; set; }
}
