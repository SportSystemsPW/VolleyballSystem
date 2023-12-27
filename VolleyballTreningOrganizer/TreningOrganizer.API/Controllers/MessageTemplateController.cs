using Microsoft.AspNetCore.Mvc;
using Volleyball.DTO.TrainingOrganizer;
using TreningOrganizer.API.IServices;
using TreningOrganizer.API.Services;
using Volleyball.Infrastructure.Database.Models;
using Microsoft.IdentityModel.Tokens;

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
            bool success = true;
            if (errors.Count == 0)
            {
                try
                {
                    messageTemplateService.UpdateMessageTemplate(editedMessageTemplate, GetTrainerId());
                }
                catch (TrainerNotAuthorizedException e)
                {
                    success = false;
                    errors.Add(e.Message);
                }
                
            }
            
            return CreateResponse(success, errors);
        }

        [HttpDelete("RemoveMessageTemplate")]
        public TrainingOrganizerResponse<bool> RemoveMessageTemplate(int id)
        {
            bool success = true;
            var errors = new List<string>();
            try
            {
                messageTemplateService.DeleteMessageTemplateById(id, GetTrainerId());
            }
            catch (TrainerNotAuthorizedException e)
            {
                success = false;
                errors.Add(e.Message);
            }
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
        [HttpGet("GetMessageTemplateDictionary")]
        public TrainingOrganizerResponse<Dictionary<string, int>> GetMessageTemplateNames()
        {
            return CreateResponse(messageTemplateService.GetMessageTemplateDictionary(GetTrainerId()));
        }

        private List<string> ValidateMessageTemplate(MessageTemplateDTO messageTemplate)
        {
            var errors = new List<string>();
            if (messageTemplate.TemplateName.IsNullOrEmpty())
                errors.Add(MessageRepository.MessageTemplateNameEmpty);
            else if (messageTemplate.TemplateName.Length > 50)
                errors.Add(MessageRepository.MessageTemplateNameTooLong);
            if (messageTemplate.Content.IsNullOrEmpty())
                errors.Add(MessageRepository.MessageTemplateEmpty);
            else if (messageTemplate.Content.Length > 300)
                errors.Add(MessageRepository.MessageTemplateTooLong);
            return new List<string>();
        }
    }
}
