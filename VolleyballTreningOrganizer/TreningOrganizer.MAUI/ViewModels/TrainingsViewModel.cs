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
using Communication = Microsoft.Maui.ApplicationModel.Communication;

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
            Trainings = new ObservableCollection<Training>();
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

        private async void Apperar()
        {
            if (isInitialLoad)
            {
                try
                {
                    var trainingDTOs = await GetDataFromAPI<List<TrainingDTO>>("Training/GetTrainingsForTrainer");
                    foreach (var trainingDTO in trainingDTOs)
                    {
                        Trainings.Add(Training.MapDTOToModel(trainingDTO));
                    }
                    isInitialLoad = false;
                }
                catch
                {
                    return;
                }
            }

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
                    FormTraining = null;
                    return;
                }
            }
            Trainings.Add(FormTraining);
            FormTraining = null;
        }

        private async void ScanSMSForResponses()
        {
            if (await Permissions.CheckStatusAsync<Permissions.Sms>() != PermissionStatus.Granted)
            {
                if (await Permissions.RequestAsync<Permissions.Sms>() != PermissionStatus.Granted)
                    return;
            }
#if ANDROID
            long lastTimeScaned = long.Parse(Preferences.Get("LastTimeScaned", "0")); //last time sms messages were scaned in long format
            string now = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            List<SMSResponse> responses = new List<SMSResponse>();
            string INBOX = "content://sms/inbox";
            string[] reqCols = new string[] { "address", "date", "body"};
            Android.Net.Uri uri = Android.Net.Uri.Parse(INBOX);
            Android.Database.ICursor cursor = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.ContentResolver.Query(uri, reqCols, null, null, null);


            if (cursor.MoveToFirst())
            {
                do
                {
                    string phone = cursor.GetString(cursor.GetColumnIndex(reqCols[0]));
                    string dateString = cursor.GetString(cursor.GetColumnIndex(reqCols[1]));
                    string body = cursor.GetString(cursor.GetColumnIndex(reqCols[2]));

                    if(long.TryParse(dateString, out long dateLong))
                    {
                        if (body.Trim().ToLower() == "yes" && dateLong > lastTimeScaned)
                        {
                            responses.Add(new SMSResponse { Phone = phone, DateTime = new DateTime(dateLong) });
                        }
                    } 
                } while (cursor.MoveToNext());
            }

            if(responses.Count > 0)
            {
                try
                {
                    await PostDataToAPI<AttendanceChangedResponse>("", responses);
                    Preferences.Set("LastTimeScaned", now);
                }
                catch
                {

                }
            }
#endif
        }
    }
}
