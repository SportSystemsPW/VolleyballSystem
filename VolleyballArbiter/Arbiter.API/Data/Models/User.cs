using System;
using System.Collections.Generic;

namespace Arbiter.API.Data.Models;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public Guid? AccountId { get; set; }

    public int? BirthYear { get; set; }

    public string? City { get; set; }

    public string? PersonalInfo { get; set; }

    public byte[]? Photo { get; set; }

    public bool Gender { get; set; }

    public byte? Height { get; set; }

    public byte? Weight { get; set; }

    public byte? JerseyNumber { get; set; }

    public int? BlockRange { get; set; }

    public int? AttackRange { get; set; }

    public string? VolleyballIdol { get; set; }

    public string? AdditionalEmail { get; set; }

    public string? Hobby { get; set; }

    public string? Phone { get; set; }

    public int PositionId { get; set; }

    public int? PhotoWidth { get; set; }

    public int? PhotoHeight { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<ForumTopic> ForumTopics { get; set; } = new List<ForumTopic>();

    public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();

    public virtual ICollection<Match> MatchMvps { get; set; } = new List<Match>();

    public virtual ICollection<Match> MatchReferees { get; set; } = new List<Match>();

    public virtual ICollection<PersonalLog> PersonalLogs { get; set; } = new List<PersonalLog>();

    public virtual Position Position { get; set; } = null!;

    public virtual Team? Team { get; set; }

    public virtual ICollection<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();

    public virtual ICollection<TypedResult> TypedResults { get; set; } = new List<TypedResult>();
}
