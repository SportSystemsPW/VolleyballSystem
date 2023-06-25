using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Volleyball.DTO.TrainingOrganizer;
using TreningOrganizer.API.IServices;

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
        public List<TrainingGroupDTO> GetTrainingGroupsForTrainer()
        {
            return trainingGroupService.GetTrainingGroupsForTrainer(GetTrainerId());
        }
        [HttpGet("GetTrainingGroupById")]
        public TrainingGroupDTO GetTrainingGroupById(int id)
        {
            return trainingGroupService.GetTrainingGroupById(id);
        }
        [HttpPost("CreateTrainingGroup")]
        public List<string> CreateTrainingGroup(TrainingGroupDTO trainingGroupDTO)
        {
            List<string> errors = ValidateTrainingGroup(trainingGroupDTO);
            if(errors.Count == 0)
            {
                trainingGroupService.InsertTrainingGroup(trainingGroupDTO, GetTrainerId());
            }
            return errors;
        }
        [HttpPut("EditTrainingGroup")]
        public List<string> EditTrainingGroup(TrainingGroupDTO trainingGroupDTO)
        {
            List<string> errors = ValidateTrainingGroup(trainingGroupDTO);
            if (errors.Count == 0)
            {
                trainingGroupService.UpdateTrainingGroup(trainingGroupDTO);
            }
            return errors;
        }
        [HttpDelete("RemoveTrainingGroup")]
        public string RemoveTrainingGroup(int id)
        {
            if (!ValidateTrainingGroupRemove(id))
            {
                return MessageRepository.CannotRemoveTrainingGroup;
            }
            trainingGroupService.DeleteTrainingGroupById(id);
            return string.Empty;
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
