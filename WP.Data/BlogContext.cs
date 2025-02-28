using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace WP.Data;

public partial class BlogContext : DbContext
{
    public BlogContext()
    {
    }

    public BlogContext(DbContextOptions<BlogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SimplenewsletterSubscription> SimplenewsletterSubscriptions { get; set; }

    public virtual DbSet<WpComment> WpComments { get; set; }

    public virtual DbSet<WpCommentmetum> WpCommentmeta { get; set; }

    public virtual DbSet<WpEemailNewsletter> WpEemailNewsletters { get; set; }

    public virtual DbSet<WpEemailNewsletterSub> WpEemailNewsletterSubs { get; set; }

    public virtual DbSet<WpLink> WpLinks { get; set; }

    public virtual DbSet<WpNewsletter> WpNewsletters { get; set; }

    public virtual DbSet<WpNewsletterEmail> WpNewsletterEmails { get; set; }

    public virtual DbSet<WpNewsletterSent> WpNewsletterSents { get; set; }

    public virtual DbSet<WpNewsletterStat> WpNewsletterStats { get; set; }

    public virtual DbSet<WpOption> WpOptions { get; set; }

    public virtual DbSet<WpPost> WpPosts { get; set; }

    public virtual DbSet<WpPostmetum> WpPostmeta { get; set; }

    public virtual DbSet<WpSimpleHistory> WpSimpleHistories { get; set; }

    public virtual DbSet<WpSimpleHistoryContext> WpSimpleHistoryContexts { get; set; }

    public virtual DbSet<WpTerm> WpTerms { get; set; }

    public virtual DbSet<WpTermRelationship> WpTermRelationships { get; set; }

    public virtual DbSet<WpTermTaxonomy> WpTermTaxonomies { get; set; }

    public virtual DbSet<WpTermmetum> WpTermmeta { get; set; }

    public virtual DbSet<WpUser> WpUsers { get; set; }

    public virtual DbSet<WpUsermetum> WpUsermeta { get; set; }

    public virtual DbSet<WpWfconfig> WpWfconfigs { get; set; }

    public virtual DbSet<WpWfcrawler> WpWfcrawlers { get; set; }

    public virtual DbSet<WpWffilechange> WpWffilechanges { get; set; }

    public virtual DbSet<WpWffilemod> WpWffilemods { get; set; }

    public virtual DbSet<WpWfloc> WpWflocs { get; set; }

    public virtual DbSet<WpWflogin> WpWflogins { get; set; }

    public virtual DbSet<WpWfls2faSecret> WpWfls2faSecrets { get; set; }

    public virtual DbSet<WpWflsRoleCount> WpWflsRoleCounts { get; set; }

    public virtual DbSet<WpWflsSetting> WpWflsSettings { get; set; }

    public virtual DbSet<WpWfnotification> WpWfnotifications { get; set; }

    public virtual DbSet<WpWftrafficrate> WpWftrafficrates { get; set; }

    public virtual DbSet<WpWfwaffailure> WpWfwaffailures { get; set; }

    public virtual DbSet<WpWpcAccesslock> WpWpcAccesslocks { get; set; }

    public virtual DbSet<WpWpcLoginFail> WpWpcLoginFails { get; set; }

    public virtual DbSet<WpWpfmBackup> WpWpfmBackups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=blog;user=root;password=Admin@123456789", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<SimplenewsletterSubscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("simplenewsletter_subscriptions")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.HasIndex(e => e.Email, "IEmail").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Confirmed).HasColumnName("confirmed");
            entity.Property(e => e.Created)
                .HasColumnType("datetime")
                .HasColumnName("created");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .HasColumnName("email");
            entity.Property(e => e.Hash)
                .HasMaxLength(32)
                .HasDefaultValueSql("''")
                .HasColumnName("hash");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<WpComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PRIMARY");

            entity
                .ToTable("wp_comments")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => new { e.CommentApproved, e.CommentDateGmt }, "comment_approved_date_gmt");

            entity.HasIndex(e => e.CommentAuthorEmail, "comment_author_email").HasAnnotation("MySql:IndexPrefixLength", new[] { 10 });

            entity.HasIndex(e => e.CommentDateGmt, "comment_date_gmt");

            entity.HasIndex(e => e.CommentParent, "comment_parent");

            entity.HasIndex(e => e.CommentPostId, "comment_post_ID");

            entity.Property(e => e.CommentId).HasColumnName("comment_ID");
            entity.Property(e => e.CommentAgent)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("comment_agent");
            entity.Property(e => e.CommentApproved)
                .HasMaxLength(20)
                .HasDefaultValueSql("'1'")
                .HasColumnName("comment_approved");
            entity.Property(e => e.CommentAuthor)
                .HasColumnType("tinytext")
                .HasColumnName("comment_author");
            entity.Property(e => e.CommentAuthorEmail)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .HasColumnName("comment_author_email");
            entity.Property(e => e.CommentAuthorIp)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .HasColumnName("comment_author_IP");
            entity.Property(e => e.CommentAuthorUrl)
                .HasMaxLength(200)
                .HasDefaultValueSql("''")
                .HasColumnName("comment_author_url");
            entity.Property(e => e.CommentContent)
                .HasColumnType("text")
                .HasColumnName("comment_content");
            entity.Property(e => e.CommentDate)
                .HasColumnType("datetime")
                .HasColumnName("comment_date");
            entity.Property(e => e.CommentDateGmt)
                .HasColumnType("datetime")
                .HasColumnName("comment_date_gmt");
            entity.Property(e => e.CommentKarma).HasColumnName("comment_karma");
            entity.Property(e => e.CommentParent).HasColumnName("comment_parent");
            entity.Property(e => e.CommentPostId).HasColumnName("comment_post_ID");
            entity.Property(e => e.CommentType)
                .HasMaxLength(20)
                .HasDefaultValueSql("'comment'")
                .HasColumnName("comment_type");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<WpCommentmetum>(entity =>
        {
            entity.HasKey(e => e.MetaId).HasName("PRIMARY");

            entity
                .ToTable("wp_commentmeta")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.CommentId, "comment_id");

            entity.HasIndex(e => e.MetaKey, "meta_key").HasAnnotation("MySql:IndexPrefixLength", new[] { 191 });

            entity.Property(e => e.MetaId)
                .ValueGeneratedNever()
                .HasColumnName("meta_id");
            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.MetaKey).HasColumnName("meta_key");
            entity.Property(e => e.MetaValue).HasColumnName("meta_value");
        });

        modelBuilder.Entity<WpEemailNewsletter>(entity =>
        {
            entity.HasKey(e => e.EemailId).HasName("PRIMARY");

            entity
                .ToTable("wp_eemail_newsletter")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.Property(e => e.EemailId)
                .ValueGeneratedNever()
                .HasColumnName("eemail_id");
            entity.Property(e => e.EemailContent)
                .HasColumnType("text")
                .HasColumnName("eemail_content")
                .UseCollation("utf8mb3_bin")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.EemailDate)
                .HasColumnType("datetime")
                .HasColumnName("eemail_date");
            entity.Property(e => e.EemailStatus)
                .HasMaxLength(3)
                .HasDefaultValueSql("'YES'")
                .IsFixedLength()
                .HasColumnName("eemail_status");
            entity.Property(e => e.EemailSubject)
                .HasColumnType("text")
                .HasColumnName("eemail_subject")
                .UseCollation("utf8mb3_bin")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<WpEemailNewsletterSub>(entity =>
        {
            entity.HasKey(e => e.EemailIdSub).HasName("PRIMARY");

            entity
                .ToTable("wp_eemail_newsletter_sub")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.Property(e => e.EemailIdSub)
                .ValueGeneratedNever()
                .HasColumnName("eemail_id_sub");
            entity.Property(e => e.EemailDateSub).HasColumnName("eemail_date_sub");
            entity.Property(e => e.EemailEmailSub)
                .HasMaxLength(250)
                .HasColumnName("eemail_email_sub");
            entity.Property(e => e.EemailNameSub)
                .HasMaxLength(250)
                .HasColumnName("eemail_name_sub");
            entity.Property(e => e.EemailStatusSub)
                .HasMaxLength(3)
                .HasColumnName("eemail_status_sub");
        });

        modelBuilder.Entity<WpLink>(entity =>
        {
            entity.HasKey(e => e.LinkId).HasName("PRIMARY");

            entity
                .ToTable("wp_links")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.LinkVisible, "link_visible");

            entity.Property(e => e.LinkId)
                .ValueGeneratedNever()
                .HasColumnName("link_id");
            entity.Property(e => e.LinkDescription)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("link_description");
            entity.Property(e => e.LinkImage)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("link_image");
            entity.Property(e => e.LinkName)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("link_name");
            entity.Property(e => e.LinkNotes)
                .HasColumnType("mediumtext")
                .HasColumnName("link_notes");
            entity.Property(e => e.LinkOwner)
                .HasDefaultValueSql("'1'")
                .HasColumnName("link_owner");
            entity.Property(e => e.LinkRating).HasColumnName("link_rating");
            entity.Property(e => e.LinkRel)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("link_rel");
            entity.Property(e => e.LinkRss)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("link_rss");
            entity.Property(e => e.LinkTarget)
                .HasMaxLength(25)
                .HasDefaultValueSql("''")
                .HasColumnName("link_target");
            entity.Property(e => e.LinkUpdated)
                .HasColumnType("datetime")
                .HasColumnName("link_updated");
            entity.Property(e => e.LinkUrl)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("link_url");
            entity.Property(e => e.LinkVisible)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Y'")
                .HasColumnName("link_visible");
        });

        modelBuilder.Entity<WpNewsletter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("wp_newsletter")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Country)
                .HasMaxLength(4)
                .HasDefaultValueSql("''")
                .HasColumnName("country");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .HasColumnName("email");
            entity.Property(e => e.Feed).HasColumnName("feed");
            entity.Property(e => e.FeedTime).HasColumnName("feed_time");
            entity.Property(e => e.Flow).HasColumnName("flow");
            entity.Property(e => e.HttpReferer)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("http_referer");
            entity.Property(e => e.Ip)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .HasColumnName("ip");
            entity.Property(e => e.List1).HasColumnName("list_1");
            entity.Property(e => e.List10).HasColumnName("list_10");
            entity.Property(e => e.List11).HasColumnName("list_11");
            entity.Property(e => e.List12).HasColumnName("list_12");
            entity.Property(e => e.List13).HasColumnName("list_13");
            entity.Property(e => e.List14).HasColumnName("list_14");
            entity.Property(e => e.List15).HasColumnName("list_15");
            entity.Property(e => e.List16).HasColumnName("list_16");
            entity.Property(e => e.List17).HasColumnName("list_17");
            entity.Property(e => e.List18).HasColumnName("list_18");
            entity.Property(e => e.List19).HasColumnName("list_19");
            entity.Property(e => e.List2).HasColumnName("list_2");
            entity.Property(e => e.List20).HasColumnName("list_20");
            entity.Property(e => e.List3).HasColumnName("list_3");
            entity.Property(e => e.List4).HasColumnName("list_4");
            entity.Property(e => e.List5).HasColumnName("list_5");
            entity.Property(e => e.List6).HasColumnName("list_6");
            entity.Property(e => e.List7).HasColumnName("list_7");
            entity.Property(e => e.List8).HasColumnName("list_8");
            entity.Property(e => e.List9).HasColumnName("list_9");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .HasColumnName("name");
            entity.Property(e => e.Profile1)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_1");
            entity.Property(e => e.Profile10)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_10");
            entity.Property(e => e.Profile11)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_11");
            entity.Property(e => e.Profile12)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_12");
            entity.Property(e => e.Profile13)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_13");
            entity.Property(e => e.Profile14)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_14");
            entity.Property(e => e.Profile15)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_15");
            entity.Property(e => e.Profile16)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_16");
            entity.Property(e => e.Profile17)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_17");
            entity.Property(e => e.Profile18)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_18");
            entity.Property(e => e.Profile19)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_19");
            entity.Property(e => e.Profile2)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_2");
            entity.Property(e => e.Profile20)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_20");
            entity.Property(e => e.Profile3)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_3");
            entity.Property(e => e.Profile4)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_4");
            entity.Property(e => e.Profile5)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_5");
            entity.Property(e => e.Profile6)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_6");
            entity.Property(e => e.Profile7)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_7");
            entity.Property(e => e.Profile8)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_8");
            entity.Property(e => e.Profile9)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("profile_9");
            entity.Property(e => e.Referrer)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .HasColumnName("referrer");
            entity.Property(e => e.Sex)
                .HasMaxLength(1)
                .HasDefaultValueSql("'n'")
                .IsFixedLength()
                .HasColumnName("sex");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .HasDefaultValueSql("'S'")
                .IsFixedLength()
                .HasColumnName("status");
            entity.Property(e => e.Surname)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .HasColumnName("surname");
            entity.Property(e => e.Test).HasColumnName("test");
            entity.Property(e => e.Token)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .HasColumnName("token");
            entity.Property(e => e.UnsubEmailId).HasColumnName("unsub_email_id");
            entity.Property(e => e.UnsubTime).HasColumnName("unsub_time");
            entity.Property(e => e.WpUserId).HasColumnName("wp_user_id");
        });

        modelBuilder.Entity<WpNewsletterEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("wp_newsletter_emails")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ClickCount).HasColumnName("click_count");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created");
            entity.Property(e => e.Editor).HasColumnName("editor");
            entity.Property(e => e.LastId).HasColumnName("last_id");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.MessageText).HasColumnName("message_text");
            entity.Property(e => e.OpenCount).HasColumnName("open_count");
            entity.Property(e => e.Options).HasColumnName("options");
            entity.Property(e => e.Preferences).HasColumnName("preferences");
            entity.Property(e => e.Private).HasColumnName("private");
            entity.Property(e => e.Query).HasColumnName("query");
            entity.Property(e => e.SendOn).HasColumnName("send_on");
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Sex)
                .HasMaxLength(1)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasColumnName("sex");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'new'")
                .HasColumnType("enum('new','sending','sent','paused')")
                .HasColumnName("status");
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("subject");
            entity.Property(e => e.Token)
                .HasMaxLength(10)
                .HasDefaultValueSql("''")
                .HasColumnName("token");
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.Track).HasColumnName("track");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .HasColumnName("type");
        });

        modelBuilder.Entity<WpNewsletterSent>(entity =>
        {
            entity.HasKey(e => new { e.EmailId, e.UserId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("wp_newsletter_sent")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.EmailId, "email_id");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Error)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .HasColumnName("error");
            entity.Property(e => e.Ip)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .HasColumnName("ip");
            entity.Property(e => e.Open).HasColumnName("open");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Time).HasColumnName("time");
        });

        modelBuilder.Entity<WpNewsletterStat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("wp_newsletter_stats")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.EmailId, "email_id");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Anchor)
                .HasMaxLength(200)
                .HasDefaultValueSql("''")
                .HasColumnName("anchor");
            entity.Property(e => e.Country)
                .HasMaxLength(4)
                .HasDefaultValueSql("''")
                .HasColumnName("country");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasDefaultValueSql("''")
                .HasColumnName("ip");
            entity.Property(e => e.LinkId).HasColumnName("link_id");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("url");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<WpOption>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PRIMARY");

            entity
                .ToTable("wp_options")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.Autoload, "autoload");

            entity.HasIndex(e => e.OptionName, "option_name").IsUnique();

            entity.Property(e => e.OptionId)
                .ValueGeneratedNever()
                .HasColumnName("option_id");
            entity.Property(e => e.Autoload)
                .HasMaxLength(20)
                .HasDefaultValueSql("'yes'")
                .HasColumnName("autoload");
            entity.Property(e => e.OptionName)
                .HasMaxLength(191)
                .HasDefaultValueSql("''")
                .HasColumnName("option_name");
            entity.Property(e => e.OptionValue).HasColumnName("option_value");
        });

        modelBuilder.Entity<WpPost>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("wp_posts")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.CommentCount).HasColumnName("comment_count");
            entity.Property(e => e.CommentStatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("'open'")
                .HasColumnName("comment_status");
            entity.Property(e => e.Guid)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("guid");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MenuOrder).HasColumnName("menu_order");
            entity.Property(e => e.PingStatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("'open'")
                .HasColumnName("ping_status");
            entity.Property(e => e.Pinged)
                .HasColumnType("text")
                .HasColumnName("pinged");
            entity.Property(e => e.PostAuthor).HasColumnName("post_author");
            entity.Property(e => e.PostContent).HasColumnName("post_content");
            entity.Property(e => e.PostContentFiltered).HasColumnName("post_content_filtered");
            entity.Property(e => e.PostDate)
                .HasColumnType("datetime")
                .HasColumnName("post_date");
            entity.Property(e => e.PostDateGmt)
                .HasColumnType("datetime")
                .HasColumnName("post_date_gmt");
            entity.Property(e => e.PostExcerpt)
                .HasColumnType("text")
                .HasColumnName("post_excerpt");
            entity.Property(e => e.PostMimeType)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .HasColumnName("post_mime_type");
            entity.Property(e => e.PostModified)
                .HasColumnType("datetime")
                .HasColumnName("post_modified");
            entity.Property(e => e.PostModifiedGmt)
                .HasColumnType("datetime")
                .HasColumnName("post_modified_gmt");
            entity.Property(e => e.PostName)
                .HasMaxLength(200)
                .HasDefaultValueSql("''")
                .HasColumnName("post_name");
            entity.Property(e => e.PostParent).HasColumnName("post_parent");
            entity.Property(e => e.PostPassword)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("post_password");
            entity.Property(e => e.PostStatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("'publish'")
                .HasColumnName("post_status");
            entity.Property(e => e.PostTitle)
                .HasColumnType("text")
                .HasColumnName("post_title");
            entity.Property(e => e.PostType)
                .HasMaxLength(20)
                .HasDefaultValueSql("'post'")
                .HasColumnName("post_type");
            entity.Property(e => e.ToPing)
                .HasColumnType("text")
                .HasColumnName("to_ping");
        });

        modelBuilder.Entity<WpPostmetum>(entity =>
        {
            entity.HasKey(e => e.MetaId).HasName("PRIMARY");

            entity
                .ToTable("wp_postmeta")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.MetaKey, "meta_key").HasAnnotation("MySql:IndexPrefixLength", new[] { 191 });

            entity.HasIndex(e => e.PostId, "post_id");

            entity.Property(e => e.MetaId).HasColumnName("meta_id");
            entity.Property(e => e.MetaKey).HasColumnName("meta_key");
            entity.Property(e => e.MetaValue).HasColumnName("meta_value");
            entity.Property(e => e.PostId).HasColumnName("post_id");
        });

        modelBuilder.Entity<WpSimpleHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("wp_simple_history")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.Date, "date");

            entity.HasIndex(e => new { e.Logger, e.Date }, "loggerdate");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Initiator)
                .HasMaxLength(16)
                .HasColumnName("initiator");
            entity.Property(e => e.Level)
                .HasMaxLength(20)
                .HasColumnName("level");
            entity.Property(e => e.Logger)
                .HasMaxLength(30)
                .HasColumnName("logger");
            entity.Property(e => e.Message)
                .HasMaxLength(255)
                .HasColumnName("message");
            entity.Property(e => e.OccasionsId)
                .HasMaxLength(32)
                .HasColumnName("occasionsID");
        });

        modelBuilder.Entity<WpSimpleHistoryContext>(entity =>
        {
            entity.HasKey(e => e.ContextId).HasName("PRIMARY");

            entity
                .ToTable("wp_simple_history_contexts")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.HistoryId, "history_id");

            entity.HasIndex(e => e.Key, "key");

            entity.Property(e => e.ContextId)
                .ValueGeneratedNever()
                .HasColumnName("context_id");
            entity.Property(e => e.HistoryId).HasColumnName("history_id");
            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<WpTerm>(entity =>
        {
            entity.HasKey(e => e.TermId).HasName("PRIMARY");

            entity
                .ToTable("wp_terms")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.Name, "name").HasAnnotation("MySql:IndexPrefixLength", new[] { 191 });

            entity.HasIndex(e => e.Slug, "slug").HasAnnotation("MySql:IndexPrefixLength", new[] { 191 });

            entity.Property(e => e.TermId)
                .ValueGeneratedNever()
                .HasColumnName("term_id");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasDefaultValueSql("''")
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasMaxLength(200)
                .HasDefaultValueSql("''")
                .HasColumnName("slug");
            entity.Property(e => e.TermGroup).HasColumnName("term_group");
        });

        modelBuilder.Entity<WpTermRelationship>(entity =>
        {
            entity.HasKey(e => new { e.ObjectId, e.TermTaxonomyId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("wp_term_relationships")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.TermTaxonomyId, "term_taxonomy_id");

            entity.Property(e => e.ObjectId).HasColumnName("object_id");
            entity.Property(e => e.TermTaxonomyId).HasColumnName("term_taxonomy_id");
            entity.Property(e => e.TermOrder).HasColumnName("term_order");
        });

        modelBuilder.Entity<WpTermTaxonomy>(entity =>
        {
            entity.HasKey(e => e.TermTaxonomyId).HasName("PRIMARY");

            entity
                .ToTable("wp_term_taxonomy")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.Taxonomy, "taxonomy");

            entity.HasIndex(e => new { e.TermId, e.Taxonomy }, "term_id_taxonomy").IsUnique();

            entity.Property(e => e.TermTaxonomyId)
                .ValueGeneratedNever()
                .HasColumnName("term_taxonomy_id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Parent).HasColumnName("parent");
            entity.Property(e => e.Taxonomy)
                .HasMaxLength(32)
                .HasDefaultValueSql("''")
                .HasColumnName("taxonomy");
            entity.Property(e => e.TermId).HasColumnName("term_id");
        });

        modelBuilder.Entity<WpTermmetum>(entity =>
        {
            entity.HasKey(e => e.MetaId).HasName("PRIMARY");

            entity
                .ToTable("wp_termmeta")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.MetaKey, "meta_key").HasAnnotation("MySql:IndexPrefixLength", new[] { 191 });

            entity.HasIndex(e => e.TermId, "term_id");

            entity.Property(e => e.MetaId)
                .ValueGeneratedNever()
                .HasColumnName("meta_id");
            entity.Property(e => e.MetaKey).HasColumnName("meta_key");
            entity.Property(e => e.MetaValue).HasColumnName("meta_value");
            entity.Property(e => e.TermId).HasColumnName("term_id");
        });

        modelBuilder.Entity<WpUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("wp_users")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.UserEmail, "user_email");

            entity.HasIndex(e => e.UserLogin, "user_login_key");

            entity.HasIndex(e => e.UserNicename, "user_nicename");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(250)
                .HasDefaultValueSql("''")
                .HasColumnName("display_name");
            entity.Property(e => e.UserActivationKey)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("user_activation_key");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .HasColumnName("user_email");
            entity.Property(e => e.UserLogin)
                .HasMaxLength(60)
                .HasDefaultValueSql("''")
                .HasColumnName("user_login");
            entity.Property(e => e.UserNicename)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .HasColumnName("user_nicename");
            entity.Property(e => e.UserPass)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("user_pass");
            entity.Property(e => e.UserRegistered)
                .HasColumnType("datetime")
                .HasColumnName("user_registered");
            entity.Property(e => e.UserStatus).HasColumnName("user_status");
            entity.Property(e => e.UserUrl)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .HasColumnName("user_url");
        });

        modelBuilder.Entity<WpUsermetum>(entity =>
        {
            entity.HasKey(e => e.UmetaId).HasName("PRIMARY");

            entity
                .ToTable("wp_usermeta")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.MetaKey, "meta_key").HasAnnotation("MySql:IndexPrefixLength", new[] { 191 });

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.UmetaId)
                .ValueGeneratedNever()
                .HasColumnName("umeta_id");
            entity.Property(e => e.MetaKey).HasColumnName("meta_key");
            entity.Property(e => e.MetaValue).HasColumnName("meta_value");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<WpWfconfig>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PRIMARY");

            entity
                .ToTable("wp_wfconfig")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Autoload)
                .HasDefaultValueSql("'yes'")
                .HasColumnType("enum('no','yes')")
                .HasColumnName("autoload");
            entity.Property(e => e.Val).HasColumnName("val");
        });

        modelBuilder.Entity<WpWfcrawler>(entity =>
        {
            entity.HasKey(e => new { e.Ip, e.PatternSig })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("wp_wfcrawlers")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Ip)
                .HasMaxLength(16)
                .HasDefaultValueSql("'0x'")
                .IsFixedLength()
                .HasColumnName("IP");
            entity.Property(e => e.PatternSig)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("patternSig");
            entity.Property(e => e.LastUpdate).HasColumnName("lastUpdate");
            entity.Property(e => e.Ptr)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("PTR");
            entity.Property(e => e.Status)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("status");
        });

        modelBuilder.Entity<WpWffilechange>(entity =>
        {
            entity.HasKey(e => e.FilenameHash).HasName("PRIMARY");

            entity
                .ToTable("wp_wffilechanges")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.FilenameHash)
                .HasMaxLength(64)
                .IsFixedLength()
                .HasColumnName("filenameHash");
            entity.Property(e => e.File)
                .HasMaxLength(1000)
                .HasColumnName("file");
            entity.Property(e => e.Md5)
                .HasMaxLength(32)
                .IsFixedLength()
                .HasColumnName("md5");
        });

        modelBuilder.Entity<WpWffilemod>(entity =>
        {
            entity.HasKey(e => e.FilenameMd5).HasName("PRIMARY");

            entity
                .ToTable("wp_wffilemods")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.FilenameMd5)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("filenameMD5");
            entity.Property(e => e.Filename)
                .HasMaxLength(1000)
                .HasColumnName("filename");
            entity.Property(e => e.IsSafeFile)
                .HasMaxLength(1)
                .HasDefaultValueSql("'?'")
                .HasColumnName("isSafeFile");
            entity.Property(e => e.KnownFile).HasColumnName("knownFile");
            entity.Property(e => e.NewMd5)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("newMD5");
            entity.Property(e => e.OldMd5)
                .HasMaxLength(16)
                .HasDefaultValueSql("'0x'")
                .IsFixedLength()
                .HasColumnName("oldMD5");
            entity.Property(e => e.RealPath)
                .HasColumnType("text")
                .HasColumnName("real_path");
            entity.Property(e => e.Shac)
                .HasMaxLength(32)
                .HasDefaultValueSql("'0x'")
                .IsFixedLength()
                .HasColumnName("SHAC");
            entity.Property(e => e.StoppedOnPosition).HasColumnName("stoppedOnPosition");
            entity.Property(e => e.StoppedOnSignature)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("stoppedOnSignature");
        });

        modelBuilder.Entity<WpWfloc>(entity =>
        {
            entity.HasKey(e => e.Ip).HasName("PRIMARY");

            entity
                .ToTable("wp_wflocs")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Ip)
                .HasMaxLength(16)
                .HasDefaultValueSql("'0x'")
                .IsFixedLength()
                .HasColumnName("IP");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("city");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasColumnName("countryCode");
            entity.Property(e => e.CountryName)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("countryName");
            entity.Property(e => e.Ctime).HasColumnName("ctime");
            entity.Property(e => e.Failed).HasColumnName("failed");
            entity.Property(e => e.Lat)
                .HasDefaultValueSql("'0.0000000'")
                .HasColumnType("float(10,7)")
                .HasColumnName("lat");
            entity.Property(e => e.Lon)
                .HasDefaultValueSql("'0.0000000'")
                .HasColumnType("float(10,7)")
                .HasColumnName("lon");
            entity.Property(e => e.Region)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("region");
        });

        modelBuilder.Entity<WpWflogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("wp_wflogins")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.HitId, "hitID");

            entity.HasIndex(e => new { e.Ip, e.Fail }, "k1");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Action)
                .HasMaxLength(40)
                .HasColumnName("action");
            entity.Property(e => e.Ctime)
                .HasColumnType("double(17,6) unsigned")
                .HasColumnName("ctime");
            entity.Property(e => e.Fail).HasColumnName("fail");
            entity.Property(e => e.HitId).HasColumnName("hitID");
            entity.Property(e => e.Ip)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("IP");
            entity.Property(e => e.Ua)
                .HasColumnType("text")
                .HasColumnName("UA");
            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

        modelBuilder.Entity<WpWfls2faSecret>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("wp_wfls_2fa_secrets")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Ctime).HasColumnName("ctime");
            entity.Property(e => e.Mode)
                .HasDefaultValueSql("'authenticator'")
                .HasColumnType("enum('authenticator')")
                .HasColumnName("mode");
            entity.Property(e => e.Recovery)
                .HasColumnType("blob")
                .HasColumnName("recovery");
            entity.Property(e => e.Secret)
                .HasColumnType("tinyblob")
                .HasColumnName("secret");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Vtime).HasColumnName("vtime");
        });

        modelBuilder.Entity<WpWflsRoleCount>(entity =>
        {
            entity.HasKey(e => new { e.SerializedRoles, e.TwoFactorInactive })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("wp_wfls_role_counts")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.SerializedRoles)
                .HasMaxLength(255)
                .HasColumnName("serialized_roles");
            entity.Property(e => e.TwoFactorInactive).HasColumnName("two_factor_inactive");
            entity.Property(e => e.UserCount).HasColumnName("user_count");
        });

        modelBuilder.Entity<WpWflsSetting>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PRIMARY");

            entity
                .ToTable("wp_wfls_settings")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Name)
                .HasMaxLength(191)
                .HasDefaultValueSql("''")
                .HasColumnName("name");
            entity.Property(e => e.Autoload)
                .HasDefaultValueSql("'yes'")
                .HasColumnType("enum('no','yes')")
                .HasColumnName("autoload");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<WpWfnotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("wp_wfnotifications")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id)
                .HasMaxLength(32)
                .HasDefaultValueSql("''")
                .HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
            entity.Property(e => e.Ctime).HasColumnName("ctime");
            entity.Property(e => e.Html)
                .HasColumnType("text")
                .HasColumnName("html");
            entity.Property(e => e.Links)
                .HasColumnType("text")
                .HasColumnName("links");
            entity.Property(e => e.New)
                .HasDefaultValueSql("'1'")
                .HasColumnName("new");
            entity.Property(e => e.Priority)
                .HasDefaultValueSql("'1000'")
                .HasColumnName("priority");
        });

        modelBuilder.Entity<WpWftrafficrate>(entity =>
        {
            entity.HasKey(e => new { e.EMin, e.Ip, e.HitType })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("wp_wftrafficrates")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.EMin).HasColumnName("eMin");
            entity.Property(e => e.Ip)
                .HasMaxLength(16)
                .HasDefaultValueSql("'0x'")
                .IsFixedLength()
                .HasColumnName("IP");
            entity.Property(e => e.HitType)
                .HasDefaultValueSql("'hit'")
                .HasColumnType("enum('hit','404')")
                .HasColumnName("hitType");
            entity.Property(e => e.Hits).HasColumnName("hits");
        });

        modelBuilder.Entity<WpWfwaffailure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("wp_wfwaffailures")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.RuleId).HasColumnName("rule_id");
            entity.Property(e => e.Throwable)
                .HasColumnType("text")
                .HasColumnName("throwable");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("timestamp");
        });

        modelBuilder.Entity<WpWpcAccesslock>(entity =>
        {
            entity.HasKey(e => e.AccesslockId).HasName("PRIMARY");

            entity
                .ToTable("wp_wpc_accesslocks")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.AccesslockId)
                .ValueGeneratedNever()
                .HasColumnName("accesslock_ID");
            entity.Property(e => e.AccesslockDate)
                .HasColumnType("datetime")
                .HasColumnName("accesslock_date");
            entity.Property(e => e.AccesslockIp)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .HasColumnName("accesslock_IP");
            entity.Property(e => e.Reason)
                .HasMaxLength(200)
                .HasColumnName("reason");
            entity.Property(e => e.ReleaseDate)
                .HasColumnType("datetime")
                .HasColumnName("release_date");
            entity.Property(e => e.Unlocked).HasColumnName("unlocked");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<WpWpcLoginFail>(entity =>
        {
            entity.HasKey(e => e.LoginAttemptId).HasName("PRIMARY");

            entity
                .ToTable("wp_wpc_login_fails")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.LoginAttemptId)
                .ValueGeneratedNever()
                .HasColumnName("login_attempt_ID");
            entity.Property(e => e.FailedPass)
                .HasMaxLength(200)
                .HasDefaultValueSql("''")
                .HasColumnName("failed_pass");
            entity.Property(e => e.FailedUser)
                .HasMaxLength(200)
                .HasDefaultValueSql("''")
                .HasColumnName("failed_user");
            entity.Property(e => e.LoginAttemptDate)
                .HasColumnType("datetime")
                .HasColumnName("login_attempt_date");
            entity.Property(e => e.LoginAttemptIp)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .HasColumnName("login_attempt_IP");
            entity.Property(e => e.Reason)
                .HasMaxLength(200)
                .HasColumnName("reason");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<WpWpfmBackup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("wp_wpfm_backup")
                .UseCollation("utf8mb4_unicode_520_ci");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BackupDate)
                .HasColumnType("text")
                .HasColumnName("backup_date");
            entity.Property(e => e.BackupName)
                .HasColumnType("text")
                .HasColumnName("backup_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
