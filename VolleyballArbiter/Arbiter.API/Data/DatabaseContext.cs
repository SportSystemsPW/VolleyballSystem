using System;
using System.Collections.Generic;
using Arbiter.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Arbiter.API.Data;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CommentLocation> CommentLocations { get; set; }

    public virtual DbSet<ForumCategory> ForumCategories { get; set; }

    public virtual DbSet<ForumTopic> ForumTopics { get; set; }

    public virtual DbSet<Invitation> Invitations { get; set; }

    public virtual DbSet<League> Leagues { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<MatchReport> MatchReports { get; set; }

    public virtual DbSet<MatchStatus> MatchStatuses { get; set; }

    public virtual DbSet<PersonalLog> PersonalLogs { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Round> Rounds { get; set; }

    public virtual DbSet<Season> Seasons { get; set; }

    public virtual DbSet<SportsVenue> SportsVenues { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamPlayer> TeamPlayers { get; set; }

    public virtual DbSet<TypedResult> TypedResults { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:VolleyballDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("tomasz1_student");

        modelBuilder.Entity<Article>(entity =>
        {
            entity.ToTable("Articles", "tomasz1_voladmin");

            entity.HasIndex(e => e.AuthorId, "IX_Articles_AuthorId");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Author).WithMany(p => p.Articles)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comments", "tomasz1_voladmin");

            entity.HasIndex(e => e.AuthorId, "IX_Comments_AuthorId");

            entity.HasIndex(e => e.CommentLocationId, "IX_Comments_CommentLocationId");

            entity.HasOne(d => d.Author).WithMany(p => p.Comments)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.CommentLocation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CommentLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<CommentLocation>(entity =>
        {
            entity.ToTable("CommentLocations", "tomasz1_voladmin");
        });

        modelBuilder.Entity<ForumCategory>(entity =>
        {
            entity.ToTable("ForumCategories", "tomasz1_voladmin");
        });

        modelBuilder.Entity<ForumTopic>(entity =>
        {
            entity.ToTable("ForumTopics", "tomasz1_voladmin");

            entity.HasIndex(e => e.AuthorId, "IX_ForumTopics_AuthorId");

            entity.HasIndex(e => e.CategoryId, "IX_ForumTopics_CategoryId");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Author).WithMany(p => p.ForumTopics).HasForeignKey(d => d.AuthorId);

            entity.HasOne(d => d.Category).WithMany(p => p.ForumTopics).HasForeignKey(d => d.CategoryId);
        });

        modelBuilder.Entity<Invitation>(entity =>
        {
            entity.ToTable("Invitations", "tomasz1_voladmin");

            entity.HasIndex(e => e.TeamId, "IX_Invitations_TeamId");

            entity.HasIndex(e => e.UserId, "IX_Invitations_UserId");

            entity.HasOne(d => d.Team).WithMany(p => p.Invitations).HasForeignKey(d => d.TeamId);

            entity.HasOne(d => d.User).WithMany(p => p.Invitations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<League>(entity =>
        {
            entity.ToTable("Leagues", "tomasz1_voladmin");

            entity.HasIndex(e => e.Name, "IX_Leagues_Name").IsUnique();
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.ToTable("Logs", "tomasz1_voladmin");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.ToTable("Matches", "tomasz1_voladmin");

            entity.HasIndex(e => e.GuestTeamId, "IX_Matches_GuestTeamId");

            entity.HasIndex(e => e.HomeTeamId, "IX_Matches_HomeTeamId");

            entity.HasIndex(e => e.LeagueId, "IX_Matches_LeagueId");

            entity.HasIndex(e => e.MvpId, "IX_Matches_MvpId");

            entity.HasIndex(e => e.RefereeId, "IX_Matches_RefereeId");

            entity.HasIndex(e => e.RoundId, "IX_Matches_RoundId");

            entity.HasIndex(e => e.VenueId, "IX_Matches_VenueId");

            entity.Property(e => e.MvpId).HasDefaultValueSql("((39))");

            entity.HasOne(d => d.GuestTeam).WithMany(p => p.MatchGuestTeams)
                .HasForeignKey(d => d.GuestTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.HomeTeam).WithMany(p => p.MatchHomeTeams)
                .HasForeignKey(d => d.HomeTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.League).WithMany(p => p.Matches).HasForeignKey(d => d.LeagueId);

            entity.HasOne(d => d.Mvp).WithMany(p => p.MatchMvps).HasForeignKey(d => d.MvpId);

            entity.HasOne(d => d.Referee).WithMany(p => p.MatchReferees).HasForeignKey(d => d.RefereeId);

            entity.HasOne(d => d.Round).WithMany(p => p.Matches).HasForeignKey(d => d.RoundId);

            entity.HasOne(d => d.Venue).WithMany(p => p.Matches).HasForeignKey(d => d.VenueId);
        });

        modelBuilder.Entity<MatchReport>(entity =>
        {
            entity
                .ToTable("ArbiterMatchReports", "res_osk_arbiter");

            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ArbiterSentence).IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Match).WithMany()
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MatchRela__Match__52593CB8");
        });

        modelBuilder.Entity<MatchStatus>(entity =>
        {
            entity
                .ToTable("ArbiterMatchStatuses", "res_osk_arbiter");

            entity.Property(e => e.MatchScore)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.MatchStatus1).HasColumnName("MatchStatus");
            entity.Property(e => e.Set1Score)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Set2Score)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Set3Score)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Set4Score)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Set5Score)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Match).WithMany()
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MatchStat__Match__5070F446");
        });

        modelBuilder.Entity<PersonalLog>(entity =>
        {
            entity.ToTable("PersonalLogs", "tomasz1_voladmin");

            entity.HasIndex(e => e.LogId, "IX_PersonalLogs_LogId");

            entity.HasIndex(e => e.UserId, "IX_PersonalLogs_UserId");

            entity.HasOne(d => d.Log).WithMany(p => p.PersonalLogs).HasForeignKey(d => d.LogId);

            entity.HasOne(d => d.User).WithMany(p => p.PersonalLogs).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("Positions", "tomasz1_voladmin");
        });

        modelBuilder.Entity<Round>(entity =>
        {
            entity.ToTable("Rounds", "tomasz1_voladmin");

            entity.HasIndex(e => e.SeasonId, "IX_Rounds_SeasonId");

            entity.HasOne(d => d.Season).WithMany(p => p.Rounds)
                .HasForeignKey(d => d.SeasonId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Season>(entity =>
        {
            entity.ToTable("Seasons", "tomasz1_voladmin");
        });

        modelBuilder.Entity<SportsVenue>(entity =>
        {
            entity.ToTable("SportsVenues", "tomasz1_voladmin");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.ToTable("Teams", "tomasz1_voladmin");

            entity.HasIndex(e => e.CaptainId, "IX_Teams_CaptainId").IsUnique();

            entity.HasIndex(e => e.LeagueId, "IX_Teams_LeagueId");

            entity.Property(e => e.IsReportedToPlay)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Captain).WithOne(p => p.Team)
                .HasForeignKey<Team>(d => d.CaptainId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.League).WithMany(p => p.Teams)
                .HasForeignKey(d => d.LeagueId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<TeamPlayer>(entity =>
        {
            entity.ToTable("TeamPlayers", "tomasz1_voladmin");

            entity.HasIndex(e => e.PlayerId, "IX_TeamPlayers_PlayerId");

            entity.HasIndex(e => e.TeamId, "IX_TeamPlayers_TeamId");

            entity.HasOne(d => d.Player).WithMany(p => p.TeamPlayers)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Team).WithMany(p => p.TeamPlayers)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<TypedResult>(entity =>
        {
            entity.ToTable("TypedResults", "tomasz1_voladmin");

            entity.HasIndex(e => e.MatchId, "IX_TypedResults_MatchId");

            entity.HasIndex(e => e.UserId, "IX_TypedResults_UserId");

            entity.HasOne(d => d.Match).WithMany(p => p.TypedResults).HasForeignKey(d => d.MatchId);

            entity.HasOne(d => d.User).WithMany(p => p.TypedResults)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users", "tomasz1_voladmin");

            entity.HasIndex(e => e.PositionId, "IX_Users_PositionId");

            entity.HasOne(d => d.Position).WithMany(p => p.Users).HasForeignKey(d => d.PositionId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
