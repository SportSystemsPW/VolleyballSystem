using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TreningOrganizer.MAUI.Models;

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

        public ObservableCollection<TrainingParticipant> TrainingParticipants { get; }
        public PaymentsViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            TrainingParticipants = new ObservableCollection<TrainingParticipant>
            {
                new TrainingParticipant
                {
                    Id = 1,
                    Name = "Antek",
                    Balance = 0,
                    Phone = "536499869"
                },
                new TrainingParticipant
                {
                    Id = 2,
                    Name = "Marek",
                    Balance = -10,
                    Phone = "502387711"
                },
                new TrainingParticipant
                {
                    Id = 3,
                    Name = "Antek",
                    Balance = 20,
                    Phone = "536499869"
                }
            };
        }
        private async void EditBalance(TrainingParticipant participant)
        {
            string changeBalanceByString = await Application.Current.MainPage.DisplayPromptAsync(string.Format("Change {0} balance by:", participant.Name), string.Empty, keyboard: Keyboard.Numeric);
            if (double.TryParse(changeBalanceByString, out double changeBalanceBy))
            {
                participant.Balance += changeBalanceBy;
                //todo api call
            }
        }
    }
}
