using System;
using System.Collections.Generic;

namespace Arbiter.API.Data.Models;

public partial class League
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
