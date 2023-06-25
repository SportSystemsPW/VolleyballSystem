using Microsoft.AspNetCore.Mvc;
using Volleyball.DTO.TrainingOrganizer;
using TreningOrganizer.API.IServices;
using TreningOrganizer.API.Services;
using Volleyball.Infrastructure.Database.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TreningOrganizer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageTemplateController : BaseController
    {
        private readonly IMessageTemplateService messageTemplateService;
        public MessageTemplateController(IMessageTemplateService messageTemplateService)
        {
            this.messageTemplateService = messageTemplateService;
        }

        [HttpPost("CreateMessageTemplate")]
        public List<string> CreateMessageTemplate(MessageTemplateDTO newMessageTemplate)
        {
            List<string> errors = ValidateMessageTemplate(newMessageTemplate);
            if (errors.Count == 0)
            {
                messageTemplateService.InsertMessageTemplate(newMessageTemplate, GetTrainerId());
            }
            return errors;
        }

        [HttpPut("EditMessageTemplate")]
        public List<string> EditMessageTemplate(int id, MessageTemplateDTO editedMessageTemplate)
        {
            List<string> errors = ValidateMessageTemplate(editedMessageTemplate);
            if (errors.Count == 0)
            {
                messageTemplateService.UpdateMessageTemplate(editedMessageTemplate);
            }
            return errors;
        }

        [HttpDelete("RemoveMessageTemplate")]
        public string RemoveMessageTemplate(int id)
        {
            if (!ValidateMessageTemplateRemove(id))
            {
                return MessageRepository.CannotRemoveMessageTemplate;
            }
            messageTemplateService.DeleteMessageTemplateById(id);
            return string.Empty;
        }
        [HttpGet("GetMessageTemplatesForTrainer")]
        public List<MessageTemplateDTO> GetMessageTemplatesForTrainer()
        {
            return messageTemplateService.GetMessageTemplatesForTrainer(GetTrainerId());
        }
        [HttpGet("GetMessageTemplateById")]
        public MessageTemplateDTO GetMessageTemplateById(int id)
        {
            return messageTemplateService.GetMessageTemplateById(id);
        }
        [HttpGet("GetMessageTemplateNames")]
        public List<string> GetMessageTemplateNames()
        {
            return messageTemplateService.GetMessageTemplateNames(GetTrainerId()); 
        }

        private List<string> ValidateMessageTemplate(MessageTemplateDTO messageTemplate)
        {
            return new List<string>();
        }

        private bool ValidateMessageTemplateRemove(int id)
        {
            return true;
        }
    }
}
