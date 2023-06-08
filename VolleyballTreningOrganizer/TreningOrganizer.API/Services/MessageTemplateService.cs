using TreningOrganizer.API.DTOs;
using TreningOrganizer.API.IRepositories;
using TreningOrganizer.API.IServices;
using TreningOrganizer.API.Repositories;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.Services
{
    public class MessageTemplateService : IMessageTemplateService
    {
        private readonly IMessageTemplateRepository messageTemplateRepository;
        public MessageTemplateService(IMessageTemplateRepository messageTemplateRepository)
        {
            this.messageTemplateRepository = messageTemplateRepository;
        }
        public void DeleteMessageTemplateById(int id)
        {
            messageTemplateRepository.DeleteMessageTemplateById(id);
        }

        public MessageTemplateDTO GetMessageTemplateById(int id)
        {
            return messageTemplateRepository.GetMessageTemplateById(id);
        }

        public List<string> GetMessageTemplateNames(int trainerId)
        {
            return messageTemplateRepository.GetMessageTemplateNames(trainerId);
        }

        public List<MessageTemplateDTO> GetMessageTemplatesForTrainer(int trainerId)
        {
            return messageTemplateRepository.GetMessageTemplatesForTrainer(trainerId);
        }

        public void InsertMessageTemplate(MessageTemplate template)
        {
            messageTemplateRepository.InsertMessageTemplate(template);
        }

        public void UpdateMessageTemplate(MessageTemplate template)
        {
            messageTemplateRepository.UpdateMessageTemplate(template);
        }
    }
}
