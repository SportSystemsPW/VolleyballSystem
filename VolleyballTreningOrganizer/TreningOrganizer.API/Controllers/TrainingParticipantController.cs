using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Volleyball.DTO.TrainingOrganizer;
using TreningOrganizer.API.IServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public List<TrainingParticipantDTO> GetTrainingParticipantsForTrainer()
        {
            return participantService.GetTrainingParticpantsForTrainer(GetTrainerId());
        }
        [HttpGet("GetTrainingParticipantById")]
        public TrainingParticipantDTO GetTrainingParticipantById(int id)
        {
            return participantService.GetTrainingParticipantById(id);
        }
        [HttpPost("CreateTraingParticipant")]
        public List<string> CreateTrainingParticipant(TrainingParticipantDTO trainingParticipantDTO)
        {
            List<string> errors = ValidateTrainingParticipant(trainingParticipantDTO);
            if(errors.Count == 0)
            {
                participantService.InsertTrainingParticipant(trainingParticipantDTO, GetTrainerId());
            }
            return errors;
        }
        [HttpPut("EditTraingParticipant")]
        public List<string> EditTrainingParticipant(TrainingParticipantDTO trainingParticipantDTO)
        {
            List<string> errors = ValidateTrainingParticipant(trainingParticipantDTO);
            if (errors.Count == 0)
            {
                participantService.UpdateTrainingParticipant(trainingParticipantDTO);
            }
            return errors;
        }
        [HttpDelete("RemoveTrainingParticipant")]
        public string RemoveTrainingParticipant(int id)
        {
            if (!ValidateTrainingParticipantRemove(id))
            {
                return MessageRepository.CannotRemoveTrainingParticipant;
            }
            participantService.DeleteTrainingParticipantById(id);
            return string.Empty;
        }


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
            bool success = false;
            List<string> errors = ValidateTrainingGroup(trainingGroupDTO);
            if (errors.Count == 0)
            {
                success = true;
                trainingGroupService.UpdateTrainingGroup(trainingGroupDTO);
            }
            return CreateResponse(success, errors);
        }
        [HttpDelete("RemoveTrainingGroup")]
        public TrainingOrganizerResponse<bool> RemoveTrainingGroup(int id)
        {
            bool success = true;
            List<string> errors = new List<string>();
            if (!ValidateTrainingGroupRemove(id))
            {
                success = false;
                errors.Add(MessageRepository.CannotRemoveTrainingGroup);
            }
            trainingGroupService.DeleteTrainingGroupById(id);
            return CreateResponse(success, errors);
        }


        private List<string> ValidateTrainingParticipant(TrainingParticipantDTO trainingParticipantDTO)
        {
            return new List<string>();
        }
        private bool ValidateTrainingParticipantRemove(int id)
        {
            return true;
        }


        private List<string> ValidateTrainingGroup(TrainingGroupDTO trainingGroupDTO)
        {
            return new List<string>();
        }
        private bool ValidateTrainingGroupRemove(int id)
        {
            return true;
        }
    }
}
