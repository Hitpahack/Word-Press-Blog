using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpNewsletterEmail
{
    public int Id { get; set; }

    public string? Message { get; set; }

    public string? MessageText { get; set; }

    public string Subject { get; set; } = null!;

    public string Type { get; set; } = null!;

    public DateTime Created { get; set; }

    public string Status { get; set; } = null!;

    public int Total { get; set; }

    public int LastId { get; set; }

    public int Sent { get; set; }

    public int SendOn { get; set; }

    public sbyte Track { get; set; }

    public sbyte Editor { get; set; }

    public string Sex { get; set; } = null!;

    public string? Query { get; set; }

    public string? Preferences { get; set; }

    public string? Options { get; set; }

    public string Token { get; set; } = null!;

    public bool Private { get; set; }

    public uint OpenCount { get; set; }

    public uint ClickCount { get; set; }
}
