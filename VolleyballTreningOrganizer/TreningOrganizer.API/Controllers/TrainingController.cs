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
        public List<TrainingDTO> GetTraingsForTrainer()
        {
            return trainingService.GetTrainingsForTrainer(GetTrainerId());
        }
        [HttpGet("GetTrainingById")]
        public TrainingDTO GetTrainingById(int id)
        {
            return trainingService.GetTrainingById(id);
        }
        [HttpPost("CreateTrainig")]
        public List<string> CreateTraining(TrainingDTO trainingDTO)
        {
            List<string> errors = ValidateTraining(trainingDTO);
            if(errors.Count == 0)
            {
                trainingService.InsertTraining(trainingDTO, GetTrainerId());
            }
            return errors;
        }
        [HttpPut("EditTraining")]
        public List<string> EditTraining(TrainingDTO trainingDTO)
        {
            List<string> errors = ValidateTraining(trainingDTO);
            if (errors.Count == 0)
            {
                trainingService.UpdateTraining(trainingDTO);
            }
            return errors;
        }
        [HttpDelete("RemoveTrainig")]
        public string RemoveTraining(int trainingId)
        {
            if (!ValidateRemoveTraining(trainingId))
            {
                return MessageRepository.CannotRemoveTraining;
            }
            trainingService.DeleteTrainingById(trainingId);
            return string.Empty;
        }
        [HttpPost("SetParticipantPresence")]
        public void SetParticipantPresence(int participantId, int trainingId, bool presence)
        {
            trainingService.SetParticipantPresence(participantId, trainingId, presence);
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
