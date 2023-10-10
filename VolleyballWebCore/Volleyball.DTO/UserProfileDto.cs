﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.DTO
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? BirthYear { get; set; }
        public string? City { get; set; }
        public string? PersonalInfo { get; set; }
        public byte[]? Photo { get; set; }
        public bool? Gender { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public int? JerseyNumber { get; set; }
        public int? BlockRange { get; set; }
        public int? AttackRange { get; set; }
        public string? VolleyballIdol { get; set; }
        public string? Hobby { get; set; }
        public string? PositionName { get; set; }

        public string? Roles { get; set; }
    }
}