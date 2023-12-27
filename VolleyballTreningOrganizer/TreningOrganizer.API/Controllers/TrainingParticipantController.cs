using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Volleyball.DTO.TrainingOrganizer;
using TreningOrganizer.API.IServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using TreningOrganizer.API.Services;
using Microsoft.IdentityModel.Tokens;

namespace TreningOrganizer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingParticipantController : BaseController
    {
        private ITrainingParticipantService participantService;
        private ITrainingGroupService trainingGroupService;
        public TrainingParticipantController(ITrainingParticipantService participantService, ITrainingGroupService trainingGroupService)
        {
            this.participantService = participantService;
            this.trainingGroupService = trainingGroupService;
        }

        [HttpGet("GetTrainingParticipantsForTrainer")]
        public TrainingOrganizerResponse<List<TrainingParticipantDTO>> GetTrainingParticipantsForTrainer()
        {
            return CreateResponse(participantService.GetTrainingParticpantsForTrainer(GetTrainerId()));
        }
        //[HttpGet("GetTrainingParticipantById")]
        //public TrainingParticipantDTO GetTrainingParticipantById(int id)
        //{
        //    return participantService.GetTrainingParticipantById(id);
        //}
        //[HttpPost("CreateTraingParticipant")]
        //public List<string> CreateTrainingParticipant(TrainingParticipantDTO trainingParticipantDTO)
        //{
        //    List<string> errors = ValidateTrainingParticipant(trainingParticipantDTO);
        //    if(errors.Count == 0)
        //    {
        //        participantService.InsertTrainingParticipant(trainingParticipantDTO, GetTrainerId());
        //    }
        //    return errors;
        //}
        [HttpPut("EditTraingParticipant")]
        public TrainingOrganizerResponse<bool> EditTrainingParticipant(TrainingParticipantDTO trainingParticipantDTO)
        {
            bool success = false;
            List<string> errors = ValidateTrainingParticipant(trainingParticipantDTO);
            if (errors.Count == 0)
            {
                success = true;
                participantService.UpdateTrainingParticipant(trainingParticipantDTO);
            }
            return CreateResponse(success, errors);
        }
        //[HttpDelete("RemoveTrainingParticipant")]
        //public string RemoveTrainingParticipant(int id)
        //{
        //    if (!ValidateTrainingParticipantRemove(id))
        //    {
        //        return MessageRepository.CannotRemoveTrainingParticipant;
        //    }
        //    participantService.DeleteTrainingParticipantById(id);
        //    return string.Empty;
        //}


        [HttpGet("GetTrainingGroupsForTrainer")]
        public TrainingOrganizerResponse<List<TrainingGroupDTO>> GetTrainingGroupsForTrainer()
        {
            return CreateResponse(trainingGroupService.GetTrainingGroupsForTrainer(GetTrainerId()));
        }
        [HttpGet("GetTrainingGroupById")]
        public TrainingOrganizerResponse<TrainingGroupDTO> GetTrainingGroupById(int id)
        {
            return CreateResponse(trainingGroupService.GetTrainingGroupById(id));
        }
        [HttpPost("CreateTrainingGroup")]
        public TrainingOrganizerResponse<int> CreateTrainingGroup(TrainingGroupDTO trainingGroupDTO)
        {
            List<string> errors = ValidateTrainingGroup(trainingGroupDTO);
            int insertedGroupId = -1;
            if(errors.Count == 0)
            {
                insertedGroupId = trainingGroupService.InsertTrainingGroup(trainingGroupDTO, GetTrainerId());
            }
            return CreateResponse(insertedGroupId, errors);
        }
        [HttpPut("EditTrainingGroup")]
        public TrainingOrganizerResponse<bool> EditTrainingGroup(TrainingGroupDTO trainingGroupDTO)
        {
            bool success = true;
            List<string> errors = ValidateTrainingGroup(trainingGroupDTO);
            if (errors.Count == 0)
            {
                try
                {
                    trainingGroupService.UpdateTrainingGroup(trainingGroupDTO, GetTrainerId());
                }
                catch (TrainerNotAuthorizedException e)
                {
                    success = false;
                    errors.Add(e.Message);
                }
            }
            else
            {
                success = false;
            }
            return CreateResponse(success, errors);
        }
        [HttpDelete("RemoveTrainingGroup")]
        public TrainingOrganizerResponse<bool> RemoveTrainingGroup(int id)
        {
            bool success = true;
            List<string> errors = new List<string>();
            try
            {
                trainingGroupService.DeleteTrainingGroupById(id, GetTrainerId());
            }
            catch(TrainerNotAuthorizedException e)
            {
                success = false;
                errors.Add(e.Message);
            }
            
            return CreateResponse(success, errors);
        }

        [HttpGet("GetTrainingGroupDictionary")]
        public TrainingOrganizerResponse<Dictionary<string, int>> GetMessageTemplateNames()
        {
            return CreateResponse(trainingGroupService.GetTrainingGroupDictionary(GetTrainerId()));
        }


        private List<string> ValidateTrainingParticipant(TrainingParticipantDTO trainingParticipantDTO)
        {
            return new List<string>();
        }

        private List<string> ValidateTrainingGroup(TrainingGroupDTO trainingGroupDTO)
        {
            var errors = new List<string>();
            if (trainingGroupDTO.Name.IsNullOrEmpty())
                errors.Add(MessageRepository.FieldEmpty("Group name"));
            else if (trainingGroupDTO.Name.Length > 50)
                errors.Add(MessageRepository.FieldTooLong("Group name", 50));
            if (trainingGroupDTO.TrainingParticipantDTOs.Count == 0)
                errors.Add(MessageRepository.EmptyGroup);
            return errors;
        }
    }
}
