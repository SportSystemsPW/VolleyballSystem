using System;
using System.Collections.Generic;

namespace Arbiter.API.Data.Models;

public partial class Season
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Round> Rounds { get; set; } = new List<Round>();
}
