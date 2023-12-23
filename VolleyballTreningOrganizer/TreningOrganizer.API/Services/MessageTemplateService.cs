﻿using Volleyball.DTO.TrainingOrganizer;
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
            MessageTemplate template = messageTemplateRepository.GetMessageTemplateById(id);
            return new MessageTemplateDTO
            {
                Id = template.Id,
                TemplateName = template.TemplateName,
                Content = template.Content
            };
        }

        public Dictionary<string, int> GetMessageTemplateDictionary(int trainerId)
        {
            return messageTemplateRepository.GetMessageTemplateDictionary(trainerId);
        }

        public List<MessageTemplateDTO> GetMessageTemplatesForTrainer(int trainerId)
        {
            return messageTemplateRepository.GetMessageTemplatesForTrainer(trainerId);
        }

        public int InsertMessageTemplate(MessageTemplateDTO template, int trainerId)
        {
            MessageTemplate messageTemplate = new MessageTemplate
            {
                TemplateName = template.TemplateName,
                Content = template.Content,
                TrainerId = trainerId
            };
            return messageTemplateRepository.InsertMessageTemplate(messageTemplate);
        }

        public void UpdateMessageTemplate(MessageTemplateDTO templateDTO)
        {
            MessageTemplate template = messageTemplateRepository.GetMessageTemplateById(templateDTO.Id);
            template.TemplateName = templateDTO.TemplateName;
            template.Content = templateDTO.Content;
            messageTemplateRepository.UpdateMessageTemplate(template);
        }
    }
}
