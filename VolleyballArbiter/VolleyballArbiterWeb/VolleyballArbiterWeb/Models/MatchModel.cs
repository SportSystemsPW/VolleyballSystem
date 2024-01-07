namespace VolleyballArbiterWeb.Models
{
    public class MatchModel
    {
        public int Id { get; set; }
        public string HomeTeam { get; set; } = string.Empty;
        public string HomeTeamLogoURL { get; set; } = string.Empty;
        public string GuestTeam { get; set; } = string.Empty;
        public string GuestTeamLogoURL { get; set; } = string.Empty;
        public string LeagueName { get; set; } = string.Empty;
        public string MatchDate { get; set; } = string.Empty;
        public string MatchTime { get; set; } = string.Empty;
    }
}
