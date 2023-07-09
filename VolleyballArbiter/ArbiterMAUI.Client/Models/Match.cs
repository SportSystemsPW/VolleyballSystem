using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbiterMAUI.Client.Models
{
    public class Match
    {
        public int Id { get; set; }
        public string HomeTeam { get; set; } = string.Empty;
        public ImageSource HomeTeamLogo { get; set; }
        public string GuestTeam { get; set; } = string.Empty;
        public ImageSource GuestTeamLogo { get; set; }
        public string LeagueName { get; set; } = string.Empty;
        public string MatchDateTime { get; set; } = string.Empty;
    }
}
