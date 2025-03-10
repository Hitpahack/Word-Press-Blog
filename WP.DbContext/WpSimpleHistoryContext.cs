using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpSimpleHistoryContext
{
    public ulong ContextId { get; set; }

    public ulong HistoryId { get; set; }

    public string? Key { get; set; }

    public string? Value { get; set; }
}
