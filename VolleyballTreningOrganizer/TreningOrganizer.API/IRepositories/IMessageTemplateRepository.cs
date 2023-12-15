﻿using Volleyball.DTO.TrainingOrganizer;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.IRepositories
{
    public interface IMessageTemplateRepository
    {
        public List<MessageTemplateDTO> GetMessageTemplatesForTrainer(int trainerId);
        public MessageTemplate GetMessageTemplateById(int id);
        public void InsertMessageTemplate(MessageTemplate template);
        public void UpdateMessageTemplate(MessageTemplate template);
        public void DeleteMessageTemplateById(int id);
        public List<string> GetMessageTemplateNames(int trainerId);
    }
}