using System;
using System.Collections.Generic;

namespace Arbiter.API.Data.Models;

public partial class Log
{
    public int Id { get; set; }

    public string? Link { get; set; }

    public string Description { get; set; } = null!;

    public DateTime Date { get; set; }

    public bool Admin { get; set; }

    public virtual ICollection<PersonalLog> PersonalLogs { get; set; } = new List<PersonalLog>();
}
