using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpNewsletter
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime Created { get; set; }

    public string Token { get; set; } = null!;

    public sbyte Feed { get; set; }

    public long FeedTime { get; set; }

    public string Country { get; set; } = null!;

    public sbyte List1 { get; set; }

    public sbyte List2 { get; set; }

    public sbyte List3 { get; set; }

    public sbyte List4 { get; set; }

    public sbyte List5 { get; set; }

    public sbyte List6 { get; set; }

    public sbyte List7 { get; set; }

    public sbyte List8 { get; set; }

    public sbyte List9 { get; set; }

    public sbyte List10 { get; set; }

    public sbyte List11 { get; set; }

    public sbyte List12 { get; set; }

    public sbyte List13 { get; set; }

    public sbyte List14 { get; set; }

    public sbyte List15 { get; set; }

    public sbyte List16 { get; set; }

    public sbyte List17 { get; set; }

    public sbyte List18 { get; set; }

    public sbyte List19 { get; set; }

    public sbyte List20 { get; set; }

    public string Profile1 { get; set; } = null!;

    public string Profile2 { get; set; } = null!;

    public string Profile3 { get; set; } = null!;

    public string Profile4 { get; set; } = null!;

    public string Profile5 { get; set; } = null!;

    public string Profile6 { get; set; } = null!;

    public string Profile7 { get; set; } = null!;

    public string Profile8 { get; set; } = null!;

    public string Profile9 { get; set; } = null!;

    public string Profile10 { get; set; } = null!;

    public string Profile11 { get; set; } = null!;

    public string Profile12 { get; set; } = null!;

    public string Profile13 { get; set; } = null!;

    public string Profile14 { get; set; } = null!;

    public string Profile15 { get; set; } = null!;

    public string Profile16 { get; set; } = null!;

    public string Profile17 { get; set; } = null!;

    public string Profile18 { get; set; } = null!;

    public string Profile19 { get; set; } = null!;

    public string Profile20 { get; set; } = null!;

    public string Referrer { get; set; } = null!;

    public string HttpReferer { get; set; } = null!;

    public int WpUserId { get; set; }

    public string Ip { get; set; } = null!;

    public sbyte Test { get; set; }

    public sbyte Flow { get; set; }

    public int UnsubEmailId { get; set; }

    public int UnsubTime { get; set; }
}
