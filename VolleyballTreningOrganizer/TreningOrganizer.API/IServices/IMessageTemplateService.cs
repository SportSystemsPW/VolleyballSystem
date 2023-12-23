﻿using Volleyball.DTO.TrainingOrganizer;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.IServices
{
    public interface IMessageTemplateService
    {
        public List<MessageTemplateDTO> GetMessageTemplatesForTrainer(int trainerId);
        public MessageTemplateDTO GetMessageTemplateById(int id);
        public int InsertMessageTemplate(MessageTemplateDTO template, int trainerId);
        public void UpdateMessageTemplate(MessageTemplateDTO template);
        public void DeleteMessageTemplateById(int id);
        public Dictionary<string, int> GetMessageTemplateDictionary(int trainerId);
    }
}
