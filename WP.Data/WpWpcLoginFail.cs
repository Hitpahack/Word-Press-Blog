using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpWpcLoginFail
{
    public long LoginAttemptId { get; set; }

    public long UserId { get; set; }

    public DateTime LoginAttemptDate { get; set; }

    public string LoginAttemptIp { get; set; } = null!;

    public string FailedUser { get; set; } = null!;

    public string FailedPass { get; set; } = null!;

    public string? Reason { get; set; }
}
