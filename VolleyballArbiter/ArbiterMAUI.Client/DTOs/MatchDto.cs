namespace ArbiterMAUI.Client.DTOs
{
    public class MatchDto
    {
        public int Id { get; set; }
        public string HomeTeam { get; set; } = string.Empty;
        public byte[]? HomeTeamLogo { get; set; } = new byte[0];
        public string GuestTeam { get; set; } = string.Empty;
        public byte[]? GuestTeamLogo { get; set; } = new byte[0];
        public string LeagueName { get; set; } = string.Empty;
        public DateTime MatchDateTime { get; set; }
    }
}
