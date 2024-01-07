using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Arbiter.API.Data.Models;

public partial class MatchReport
{
    [Key]
    public int Id { get; set; }

    public int MatchId { get; set; }

    public string Action { get; set; } = null!;

    public string ArbiterSentence { get; set; } = null!;

    public int Set { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual Match Match { get; set; } = null!;
}
