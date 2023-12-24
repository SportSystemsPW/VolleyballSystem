using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volleyball.DTO.TrainingOrganizer;
using TreningOrganizer.API.IServices;

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
        [HttpGet("GetTrainingsForTrainer")]
        public TrainingOrganizerResponse<List<TrainingDTO>> GetTraingsForTrainer()
        {
            return CreateResponse(trainingService.GetTrainingsForTrainer(GetTrainerId()));
        }
        [HttpGet("GetTrainingById")]
        public TrainingOrganizerResponse<TrainingDTO> GetTrainingById(int id)
        {
            return CreateResponse(trainingService.GetTrainingById(id));
        }
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
        //[HttpPut("EditTraining")]
        //public List<string> EditTraining(TrainingDTO trainingDTO)
        //{
        //    List<string> errors = ValidateTraining(trainingDTO);
        //    if (errors.Count == 0)
        //    {
        //        trainingService.UpdateTraining(trainingDTO);
        //    }
        //    return errors;
        //}
        //[HttpDelete("RemoveTrainig")]
        //public string RemoveTraining(int trainingId)
        //{
        //    if (!ValidateRemoveTraining(trainingId))
        //    {
        //        return MessageRepository.CannotRemoveTraining;
        //    }
        //    trainingService.DeleteTrainingById(trainingId);
        //    return string.Empty;
        //}
        [HttpPut("SetParticipantsPresence")]
        public TrainingOrganizerResponse<bool> SetParticipantPresence(TrainingPresencesDTO trainingPresencesDTO)
        {
            trainingService.SetParticipantPresence(trainingPresencesDTO);
            return CreateResponse(true);
        }

        [HttpPost("ProcessSMSResponses")]
        public TrainingOrganizerResponse<List<AttendanceChangedResponseDTO>> ProcessSMSResponses(List<SMSResponseDTO> smsResponseDTOs)
        {
            return CreateResponse(trainingService.ProcessSMSResponses(smsResponseDTOs, GetTrainerId()));
        }

        private List<string> ValidateTraining(TrainingDTO trainingDTO)
        {
            return new List<string>();
        }
        private bool ValidateRemoveTraining(int id)
        {
            return true;
        }
    }
}
