﻿using System;
using System.Collections.Generic;

namespace WP.Data;

public partial class WpTermRelationship
{
    public ulong ObjectId { get; set; }

    public ulong TermTaxonomyId { get; set; }

    public int TermOrder { get; set; }
}
