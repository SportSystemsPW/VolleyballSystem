namespace TreningOrganizer.API.DTOs
{
    public class MessageTemplateDTO
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string TemplateName { get; set; } = string.Empty;
    }
}
