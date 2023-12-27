using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Volleyball.DTO.TrainingOrganizer;
using TreningOrganizer.API.IRepositories;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.Repositories
{
    public class MessageTemplateRepository : IMessageTemplateRepository
    {
        private DbSet<MessageTemplate> messageTemplates;
        private readonly VolleyballContext context;
        public MessageTemplateRepository(VolleyballContext context)
        {
            this.context = context;
            messageTemplates = context.MessageTemplates;
        }
        public void DeleteMessageTemplateById(int id, int trainerId)
        {
            MessageTemplate messageTemplate = messageTemplates.FirstOrDefault(mt => mt.Id == id);
            if (messageTemplate != null)
            {
                if (trainerId != messageTemplate.TrainerId)
                    throw new TrainerNotAuthorizedException(MessageRepository.CannotRemoveObject("message template"));
                messageTemplates.Remove(messageTemplate);
                context.SaveChanges();
            }
        }

        public MessageTemplate GetMessageTemplateById(int id)
        {
            MessageTemplate messageTemplate = messageTemplates.FirstOrDefault(mt => mt.Id == id);
            if(messageTemplate == null)
            {
                throw new Exception();
            }
            return messageTemplate;
        }

        public List<MessageTemplateDTO> GetMessageTemplatesForTrainer(int trainerId)
        {
            return messageTemplates.Where(mt => mt.TrainerId == trainerId).Select(mt => new MessageTemplateDTO
            {
                Id = mt.Id,
                TemplateName = mt.TemplateName,
                Content = mt.Content
            }).ToList();
        }

        public int InsertMessageTemplate(MessageTemplate template)
        {
            messageTemplates.Add(template);
            context.SaveChanges();
            return template.Id;
        }

        public void UpdateMessageTemplate(MessageTemplate template)
        {
            messageTemplates.Update(template);
            context.SaveChanges();
        }

        public Dictionary<string, int> GetMessageTemplateDictionary(int trainerId)
        {
            return messageTemplates.Where(mt => mt.TrainerId == trainerId).ToDictionary(mt => mt.TemplateName, mt => mt.Id);
        }
    }
}
