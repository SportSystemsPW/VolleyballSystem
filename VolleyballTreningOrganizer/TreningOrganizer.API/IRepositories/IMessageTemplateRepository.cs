using Volleyball.DTO.TrainingOrganizer;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.IRepositories
{
    public interface IMessageTemplateRepository
    {
        public List<MessageTemplateDTO> GetMessageTemplatesForTrainer(int trainerId);
        public MessageTemplate GetMessageTemplateById(int id);
        public int InsertMessageTemplate(MessageTemplate template);
        public void UpdateMessageTemplate(MessageTemplate template);
        public void DeleteMessageTemplateById(int id, int trainerId);
        public Dictionary<string, int> GetMessageTemplateDictionary(int trainerId);
    }
}
