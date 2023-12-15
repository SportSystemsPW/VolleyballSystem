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
    [QueryProperty("FormTraining", "FormTraining")]
    public class TrainingsViewModel : BaseViewModel
    {
        public Training FormTraining { get; set; }
        public ICommand DetailsCommand
        {
            get
            {
                return new Command<Training>(ShowDetails);
            }
        }
        public ICommand CreateCommand
        {
            get
            {
                return new Command(CreateTraining);
            }
        }

        public ICommand AppearCommand
        {
            get
            {
                return new Command(Apperar);
            }
        }

        public ICommand ScanSMSCommand
        {
            get
            {
                return new Command(ScanSMSForResponses);
            }
        }

        public ObservableCollection<Training> Trainings { get; }
        public TrainingsViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Trainings = new ObservableCollection<Training>
            {
                new Training()
                {
                    Id = 1,
                    Name = "Test",
                    Date = new DateTime(2023, 12, 10, 10, 0, 0),
                    ParticipantsPresent = 0,
                    ParticipantsTotal = 10
                },
                new Training()
                {
                    Id = 2,
                    Name = "Test2",
                    Date = new DateTime(2023, 12, 11, 12, 0, 0),
                    ParticipantsPresent = 0,
                    ParticipantsTotal = 15
                },
                new Training()
                {
                    Id = 3,
                    Name = "Test3",
                    Date = new DateTime(2023, 12, 12, 9, 0, 0),
                    ParticipantsPresent = 0,
                    ParticipantsTotal = 20
                }
            };
        }
        private async void ShowDetails(Training training)
        {
            var parameters = new Dictionary<string, object>
            {
                { "Training", training }
            };
            await Shell.Current.GoToAsync("trainingDetails", parameters);
        }

        private async void CreateTraining()
        {
            await Shell.Current.GoToAsync("trainingForm");
        }

        private void Apperar()
        {
            if (FormTraining == null)
            {
                return;
            }

            foreach (Training training in Trainings)
            {
                if (training.Id == FormTraining.Id)
                {
                    training.ParticipantsPresent = FormTraining.ParticipantsPresent;
                    training.ParticipantsTotal = FormTraining.ParticipantsTotal;
                    return;
                }
            }
            Trainings.Add(FormTraining);
        }

        private void ScanSMSForResponses()
        {

        }
    }
}
