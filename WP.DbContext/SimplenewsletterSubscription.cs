using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class SimplenewsletterSubscription
{
    public uint Id { get; set; }

    public string? Name { get; set; }

    public string Email { get; set; } = null!;

    public string Hash { get; set; } = null!;

    public int? Confirmed { get; set; }

    public DateTime? Created { get; set; }
}
