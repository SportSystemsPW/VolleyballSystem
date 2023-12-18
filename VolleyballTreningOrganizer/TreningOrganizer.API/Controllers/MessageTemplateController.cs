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
        public TrainingOrganizerResponse<int> CreateMessageTemplate([FromBody] MessageTemplateDTO newMessageTemplate)
        {
            List<string> errors = ValidateMessageTemplate(newMessageTemplate);
            int insertedTemplateId = -1;
            if (errors.Count == 0)
            {
                insertedTemplateId = messageTemplateService.InsertMessageTemplate(newMessageTemplate, GetTrainerId());
            }
            return CreateResponse(insertedTemplateId, errors);
        }

        [HttpPut("EditMessageTemplate")]
        public TrainingOrganizerResponse<bool> EditMessageTemplate([FromBody] MessageTemplateDTO editedMessageTemplate)
        {
            List<string> errors = ValidateMessageTemplate(editedMessageTemplate);
            bool success = false;
            if (errors.Count == 0)
            {
                messageTemplateService.UpdateMessageTemplate(editedMessageTemplate);
                success = true;
            }
            return CreateResponse(success, errors);
        }

        [HttpDelete("RemoveMessageTemplate")]
        public TrainingOrganizerResponse<bool> RemoveMessageTemplate(int id)
        {
            bool success = true;
            var errors = new List<string>();
            if (!ValidateMessageTemplateRemove(id))
            {
                errors.Add(MessageRepository.CannotRemoveMessageTemplate);
                success = false;
            }
            messageTemplateService.DeleteMessageTemplateById(id);
            return CreateResponse(success, errors);
        }
        [HttpGet("GetMessageTemplatesForTrainer")]
        public TrainingOrganizerResponse<List<MessageTemplateDTO>> GetMessageTemplatesForTrainer()
        {
            return CreateResponse(messageTemplateService.GetMessageTemplatesForTrainer(GetTrainerId()));
        }
        [HttpGet("GetMessageTemplateById")]
        public TrainingOrganizerResponse<MessageTemplateDTO> GetMessageTemplateById(int id)
        {
            return CreateResponse(messageTemplateService.GetMessageTemplateById(id));
        }
        [HttpGet("GetMessageTemplateNames")]
        public TrainingOrganizerResponse<List<string>> GetMessageTemplateNames()
        {
            return CreateResponse(messageTemplateService.GetMessageTemplateNames(GetTrainerId()));
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
