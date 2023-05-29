﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Volleyball.Infrastructure.Database.Models;

public partial class VolleyballContext : DbContext
{
 

    public VolleyballContext(DbContextOptions<VolleyballContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }


    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamPlayer> TeamPlayers { get; set; }

    public virtual DbSet<ForumCategory> ForumCategories { get; set; }

    public virtual DbSet<Round> Rounds { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<League> Leagues { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<PersonalLog> PersonalLogs { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<CommentLocation> CommentLocations { get; set; }



    public virtual DbSet<SportsVenue> SportsVenues { get; set; }

    public virtual DbSet<Position> Positions { get; set; }



    public virtual DbSet<Season> Seasons { get; set; }


    public virtual DbSet<ForumTopic> ForumTopics { get; set; }

    public virtual DbSet<User> Users { get; set; }



    public virtual DbSet<TypedResult> TypedResults { get; set; }

    public virtual DbSet<Invitation> Invitations { get; set; }

 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Author).WithMany(p => p.Articles)
               .HasForeignKey(d => d.AuthorId)
               .OnDelete(DeleteBehavior.ClientSetNull);
           
        });

        modelBuilder.Entity<Team>(entity =>
        {
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



        modelBuilder.Entity<League>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<TeamPlayer>(entity =>
        {
            entity.HasOne(d => d.Team).WithMany(p => p.TeamPlayers)
          .HasForeignKey(d => d.TeamId)
          .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Player).WithMany(p => p.TeamPlayers)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
             
        });

        modelBuilder.Entity<TypedResult>(entity =>
        {

            entity.HasOne(d => d.Match).WithMany(p => p.TypedResults)
                .HasForeignKey(d => d.MatchId);


            entity.HasOne(d => d.User).WithMany(p => p.TypedResults)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        
        });

        modelBuilder.Entity<Match>(entity =>
        {

            entity.Property(e => e.MvpId)
                .HasDefaultValueSql("((39))");

            entity.HasOne(d => d.Mvp).WithMany(p => p.MVPMatches)
              .HasForeignKey(d => d.MvpId);
 
            entity.HasOne(d => d.Referee).WithMany(p => p.RefereeMatches)
                .HasForeignKey(d => d.RefereeId);
 
            entity.HasOne(m => m.HomeTeam)
            .WithMany(t => t.HomeMatches)
            .HasForeignKey(m => m.HomeTeamId)
            .OnDelete(DeleteBehavior.ClientSetNull); 

            entity.HasOne(m => m.GuestTeam)
            .WithMany(t => t.GuestMatches)
            .HasForeignKey(m => m.GuestTeamId)
            .OnDelete(DeleteBehavior.ClientSetNull);  

 
        });

        modelBuilder.Entity<ForumTopic>(entity =>
        {

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<Round>(entity =>
        {
            entity.HasOne(d => d.Season).WithMany(p => p.Rounds)
               .HasForeignKey(d => d.SeasonId)
               .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasOne(d => d.Author).WithMany(p => p.Comments)
             .HasForeignKey(d => d.AuthorId)
             .OnDelete(DeleteBehavior.ClientSetNull);


            entity.HasOne(d => d.CommentLocation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CommentLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull);
           
        });

        modelBuilder.Entity<Invitation>(entity =>
        {

            entity.HasOne(d => d.User).WithMany(p => p.Invitations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
    

        });

        modelBuilder.Entity<PersonalLog>(entity =>
        {

            entity.HasOne(d => d.Log).WithMany(p => p.PersonalLogs)
              .HasForeignKey(d => d.LogId);


            entity.HasOne(d => d.User).WithMany(p => p.PersonalLogs)
                .HasForeignKey(d => d.UserId);

        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
