﻿using System;
using System.Collections.Generic;

namespace Volleyball.Infrastructure.Database.Models
{
    public partial class SportsVenue
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}
