using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TreningOrganizer.API.DTOs;
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
        public void DeleteMessageTemplateById(int id)
        {
            MessageTemplate messageTemplate = messageTemplates.FirstOrDefault(mt => mt.Id == id);
            if (messageTemplate != null)
            {
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

        public void InsertMessageTemplate(MessageTemplate template)
        {
            messageTemplates.Add(template);
            context.SaveChanges();
        }

        public void UpdateMessageTemplate(MessageTemplate template)
        {
            messageTemplates.Update(template);
            context.SaveChanges();
        }

        public List<string> GetMessageTemplateNames(int trainerId)
        {
            return messageTemplates.Where(mt => mt.TrainerId == trainerId).Select(mt => mt.TemplateName).ToList();
        }
    }
}
