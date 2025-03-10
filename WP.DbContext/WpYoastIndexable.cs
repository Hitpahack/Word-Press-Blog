using System;
using System.Collections.Generic;

namespace WP.DataContext;

public partial class WpYoastIndexable
{
    public uint Id { get; set; }

    public string? Permalink { get; set; }

    public string? PermalinkHash { get; set; }

    public long? ObjectId { get; set; }

    public string ObjectType { get; set; } = null!;

    public string? ObjectSubType { get; set; }

    public long? AuthorId { get; set; }

    public long? PostParent { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? BreadcrumbTitle { get; set; }

    public string? PostStatus { get; set; }

    public bool? IsPublic { get; set; }

    public bool? IsProtected { get; set; }

    public bool? HasPublicPosts { get; set; }

    public uint? NumberOfPages { get; set; }

    public string? Canonical { get; set; }

    public string? PrimaryFocusKeyword { get; set; }

    public int? PrimaryFocusKeywordScore { get; set; }

    public int? ReadabilityScore { get; set; }

    public bool? IsCornerstone { get; set; }

    public bool? IsRobotsNoindex { get; set; }

    public bool? IsRobotsNofollow { get; set; }

    public bool? IsRobotsNoarchive { get; set; }

    public bool? IsRobotsNoimageindex { get; set; }

    public bool? IsRobotsNosnippet { get; set; }

    public string? TwitterTitle { get; set; }

    public string? TwitterImage { get; set; }

    public string? TwitterDescription { get; set; }

    public string? TwitterImageId { get; set; }

    public string? TwitterImageSource { get; set; }

    public string? OpenGraphTitle { get; set; }

    public string? OpenGraphDescription { get; set; }

    public string? OpenGraphImage { get; set; }

    public string? OpenGraphImageId { get; set; }

    public string? OpenGraphImageSource { get; set; }

    public string? OpenGraphImageMeta { get; set; }

    public int? LinkCount { get; set; }

    public int? IncomingLinkCount { get; set; }

    public uint? ProminentWordsVersion { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public long BlogId { get; set; }

    public string? Language { get; set; }

    public string? Region { get; set; }

    public string? SchemaPageType { get; set; }

    public string? SchemaArticleType { get; set; }

    public bool? HasAncestors { get; set; }

    public int? EstimatedReadingTimeMinutes { get; set; }

    public int? Version { get; set; }

    public DateTime? ObjectLastModified { get; set; }

    public DateTime? ObjectPublishedAt { get; set; }

    public int? InclusiveLanguageScore { get; set; }
}
