using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volleyball.DTO.TrainingOrganizer;
using TreningOrganizer.API.IServices;
using Microsoft.AspNetCore.Authorization;

namespace TreningOrganizer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingController : BaseController
    {
        private readonly ITrainingService trainingService;
        public TrainingController(ITrainingService trainingService)
        {
            this.trainingService = trainingService;
        }
        [Authorize]
        [HttpGet("GetTrainingsForTrainer")]
        public TrainingOrganizerResponse<List<TrainingDTO>> GetTraingsForTrainer()
        {
            return CreateResponse(trainingService.GetTrainingsForTrainer(GetTrainerId()));
        }
        [Authorize]
        [HttpGet("GetTrainingById")]
        public TrainingOrganizerResponse<TrainingDTO> GetTrainingById(int id)
        {
            return CreateResponse(trainingService.GetTrainingById(id));
        }
        [Authorize]
        [HttpPost("CreateTrainig")]
        public TrainingOrganizerResponse<int> CreateTraining(TrainingDTO trainingDTO)
        {
            int createdTrainingId = -1;
            List<string> errors = ValidateTraining(trainingDTO);
            if(errors.Count == 0)
            {
                createdTrainingId = trainingService.InsertTraining(trainingDTO, GetTrainerId());
            }
            return CreateResponse(createdTrainingId, errors);
        }
        [Authorize]
        [HttpPut("SetParticipantsPresence")]
        public TrainingOrganizerResponse<bool> SetParticipantPresence(TrainingPresencesDTO trainingPresencesDTO)
        {
            List<string> errors = new List<string>();
            bool success = true;
            try
            {
                trainingService.SetParticipantPresence(trainingPresencesDTO, GetTrainerId());
            }
            catch(TrainerNotAuthorizedException e)
            {
                errors.Add(e.Message);
                success = false;
            }
            
            return CreateResponse(success, errors);
        }
        [Authorize]
        [HttpPost("ProcessSMSResponses")]
        public TrainingOrganizerResponse<List<AttendanceChangedResponseDTO>> ProcessSMSResponses(List<SMSResponseDTO> smsResponseDTOs)
        {
            return CreateResponse(trainingService.ProcessSMSResponses(smsResponseDTOs, GetTrainerId()));
        }

        private List<string> ValidateTraining(TrainingDTO trainingDTO)
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(trainingDTO.Location))
                errors.Add(MessageRepository.FieldEmpty("location"));
            else if (trainingDTO.Location.Length > 50)
                errors.Add(MessageRepository.FieldTooLong("location", 50));
            if (string.IsNullOrEmpty(trainingDTO.Name))
                errors.Add(MessageRepository.FieldEmpty("name"));
            else if (trainingDTO.Name.Length > 50)
                errors.Add(MessageRepository.FieldTooLong("name", 50));
            if (trainingDTO.ParticipantDTOs.Count == 0)
                errors.Add(MessageRepository.EmptyTraining);

            return errors;
        }
    }
}
