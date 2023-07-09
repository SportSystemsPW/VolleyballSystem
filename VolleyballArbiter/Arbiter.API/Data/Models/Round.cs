using System;
using System.Collections.Generic;

namespace Arbiter.API.Data.Models;

public partial class Round
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int SeasonId { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    public virtual Season Season { get; set; } = null!;
}
