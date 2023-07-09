using System;
using System.Collections.Generic;

namespace Arbiter.API.Data.Models;

public partial class TypedResult
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int MatchId { get; set; }

    public byte Score1 { get; set; }

    public byte Score2 { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
