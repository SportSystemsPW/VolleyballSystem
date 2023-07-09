using System;
using System.Collections.Generic;

namespace Arbiter.API.Data.Models;

public partial class PersonalLog
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int LogId { get; set; }

    public virtual Log Log { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
