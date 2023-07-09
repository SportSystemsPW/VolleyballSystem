using System;
using System.Collections.Generic;

namespace Arbiter.API.Data.Models;

public partial class TeamPlayer
{
    public int Id { get; set; }

    public int TeamId { get; set; }

    public int PlayerId { get; set; }

    public DateTime JoinDate { get; set; }

    public virtual User Player { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
