using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using TreningOrganizer.MAUI.Models;
using Volleyball.DTO.TrainingOrganizer;

namespace TreningOrganizer.MAUI.ViewModels
{
    public class PaymentsViewModel : BaseViewModel
    {
        public ICommand EditCommand
        {
            get
            {
                return new Command<TrainingParticipant>(EditBalance);
            }
        }

        public ICommand AppearCommand
        {
            get
            {
                return new Command(Appear);
            }
        }

        public ObservableCollection<TrainingParticipant> TrainingParticipants { get; }
        public PaymentsViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            TrainingParticipants = new ObservableCollection<TrainingParticipant>();
        }
        private async void EditBalance(TrainingParticipant participant)
        {
            string changeBalanceByString = await Application.Current.MainPage.DisplayPromptAsync(string.Format("Change {0} balance by:", participant.Name), string.Empty, keyboard: Keyboard.Numeric);
            if (double.TryParse(changeBalanceByString, out double changeBalanceBy))
            {
                participant.Balance += changeBalanceBy;

                try
                {
                    await PutDataToAPI("TrainingParticipant/EditTraingParticipant", TrainingParticipant.MapModelToDTO(participant));
                }
                catch
                {
                    
                }
            }
        }

        private async void Appear()
        {
            if (isInitialLoad)
            {
                try
                {
                    var trainingParticipantDTOs = await GetDataFromAPI<List<TrainingParticipantDTO>>("TrainingParticipant/GetTrainingParticipantsForTrainer");
                    foreach (var participantDTO in trainingParticipantDTOs)
                    {
                        TrainingParticipants.Add(TrainingParticipant.MapDTOToModel(participantDTO));
                    }
                    isInitialLoad = false;
                }
                catch
                {
                    return;
                }
            }
        }
    }
}
