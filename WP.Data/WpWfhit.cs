using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpWfhit
{
    public uint Id { get; set; }

    public double AttackLogTime { get; set; }

    public double Ctime { get; set; }

    public byte[]? Ip { get; set; }

    public sbyte? JsRun { get; set; }

    public int StatusCode { get; set; }

    public sbyte IsGoogle { get; set; }

    public uint UserId { get; set; }

    public byte NewVisit { get; set; }

    public string? Url { get; set; }

    public string? Referer { get; set; }

    public string? Ua { get; set; }

    public string Action { get; set; } = null!;

    public string? ActionDescription { get; set; }

    public string? ActionData { get; set; }
}
