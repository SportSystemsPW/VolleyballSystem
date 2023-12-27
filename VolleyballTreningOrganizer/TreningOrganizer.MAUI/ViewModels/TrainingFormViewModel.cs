using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TreningOrganizer.MAUI.Models;
using Volleyball.DTO.TrainingOrganizer;

namespace TreningOrganizer.MAUI.ViewModels
{
    [QueryProperty("Training", "Training")]
    [QueryProperty("formTraining", "formTraining")]
    [QueryProperty("members", "members")]
    public class TrainingFormViewModel : BaseViewModel
    {
        //training form and training details was supposed to be single view but binding to IsVisible and IsEnabled doesn't work
        public string Title { get; set; }
        public Training Training { get; set; }
        public Training FormTraining { get; set; }
        public Training formTraining { get; set; }
        public ObservableCollection<Models.Contact> Members { get; set; }
        public List<Models.Contact> members { get; set; }
        public bool IsDetailsView { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public TimeSpan Time { get; set; } = DateTime.Now.TimeOfDay;
        public bool InformHowToRespond { get; set; } = false;
        public int NameMaxLength { get; } = 50;
        public int LocationMaxLength { get; } = 50;
        private Dictionary<string, int> MessageTemplatesDropdown { get; set; }
        private Dictionary<string, int> GroupsDropdown { get; set; }
        public TrainingFormViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Members = new ObservableCollection<Models.Contact>();
            FormTraining = new Training();
        }
        public ICommand CancelCommand
        {
            get
            {
                return new Command(Cancel);
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                return new Command(Save);
            }
        }
        public ICommand FillFormCommand
        {
            get
            {
                return new Command(FillForm);
            }
        }
        public ICommand DeleteCommand
        {
            get
            {
                return new Command<Models.Contact>(DeleteContact);
            }
        }
        public ICommand ContactsCommand
        {
            get
            {
                return new Command(SelectConntacts);
            }
        }

        public ICommand TemplateCommand
        {
            get
            {
                return new Command(SelectTemplate);
            }
        }

        public ICommand GroupCommand
        {
            get
            {
                return new Command(SelectGroup);
            }
        }

        private async void SelectConntacts()
        {
            if (await Permissions.CheckStatusAsync<Permissions.ContactsRead>() != PermissionStatus.Granted)
            {
                if (await Permissions.RequestAsync<Permissions.ContactsRead>() != PermissionStatus.Granted)
                    return;
            }

            members = Members.ToList();
            var parameters = new Dictionary<string, object>
            {
                { "Training", Training },
                { "fromTraining", formTraining },
                { "members", members }
            };
            await Shell.Current.GoToAsync("contacts", parameters);
        }

        private void DeleteContact(Models.Contact contact)
        {
            Members.Remove(contact);
        }

        private async void Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void Save()
        {
            if (IsDetailsView)
            {
                var presentMembers = Members.Where(m => m.Present);
                FormTraining.ParticipantsPresent = presentMembers.Count();
            }
            else
            {
                bool validate = true;
                List<string> errors = new List<string>();
                if (string.IsNullOrEmpty(FormTraining.Name))
                {
                    validate = false;
                    errors.Add("Training name can't be empty");
                }
                else if (FormTraining.Name.Length > NameMaxLength)
                {
                    validate = false;
                    errors.Add($"Training name can't be longer than {NameMaxLength} characters");
                }
                if (string.IsNullOrEmpty(FormTraining.Location))
                {
                    validate = false;
                    errors.Add("Location can't be empty");
                }
                else if (FormTraining.Location.Length > LocationMaxLength)
                {
                    validate = false;
                    errors.Add($"Location can't be longer than {LocationMaxLength} characters");
                }
                if (Members.Count == 0)
                {
                    validate = false;
                    errors.Add("Training must have at least one participant");
                }
                if (!validate)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", string.Join('\n', errors), "OK");
                    return;
                }

                if (await Permissions.CheckStatusAsync<Permissions.Sms>() != PermissionStatus.Granted)
                {
                    if (await Permissions.RequestAsync<Permissions.Sms>() != PermissionStatus.Granted)
                        return;
                }

                bool proceed = await CheckMessageForVariables();
                if (!proceed) return;

                FormTraining.ParticipantsPresent = 0;
                FormTraining.Date = Date.Date + Time;

                if (Sms.Default.IsComposeSupported)
                {
                    List<string> recipients = new List<string>();
                    foreach (var contact in Members)
                    {
                        recipients.Add(contact.Phone);
                    }

                    FormTraining.Message = FormTraining.Message.Replace("{date}", FormTraining.DateString).Replace("{price}", FormTraining.Price.ToString()).Replace("{location}", FormTraining.Location);
                    if (InformHowToRespond)
                    {
                        FormTraining.Message += "\n\nTo confirm that you will attend this training reply YES to this message";
                    }

                    var message = new SmsMessage(FormTraining.Message, recipients);

                    await Sms.Default.ComposeAsync(message);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", "SMS messages will not be send", "OK");
                }               
            }
            FormTraining.ParticipantsTotal = Members.Count;

            try
            {
                if (!IsDetailsView)
                {
                    int insertedTrainigId = await PostDataToAPI<int>("Training/CreateTrainig", Training.MapModelToDTO(FormTraining, Members));
                    FormTraining.Id = insertedTrainigId;
                }
                else
                {
                    List<TrainingTrainingParticipantDTO> participantDTOs = new List<TrainingTrainingParticipantDTO>();
                    foreach (var contact in Members)
                    {
                        participantDTOs.Add(new TrainingTrainingParticipantDTO
                        {
                            Id = contact.Id,
                            Presence = contact.Present
                        });
                    }
                    var presencesDTO = new TrainingPresencesDTO
                    {
                        TrainingId = FormTraining.Id,
                        ParticipantDTOs = participantDTOs
                    };
                    await PutDataToAPI("Training/SetParticipantsPresence", presencesDTO);
                }
            }
            catch
            {
                await Shell.Current.GoToAsync("..");
            }

            var parameters = new Dictionary<string, object>
            {
                { "FormTraining", FormTraining }
            };
            await Shell.Current.GoToAsync("..", parameters);
        }

        private async Task<bool> CheckMessageForVariables()
        {
            bool proceed = true;
            if(string.IsNullOrEmpty(FormTraining.Message))
            {
                string[] variables = { "{date}", "{location}", "{price}" };
                List<string> missingVariables = new List<string>();
                foreach (string variable in variables)
                {
                    if (!FormTraining.Message.Contains(variable))
                    {
                        missingVariables.Add(variable);
                    }
                }

                if (missingVariables.Count > 0)
                {
                    string warningMessage = string.Format("Your message does not contain {0} variable{1}.\nDo you want to proceed anyway?", string.Join(", ", missingVariables), missingVariables.Count == 1 ? "" : "s");
                    proceed = await Application.Current.MainPage.DisplayAlert("Warning", warningMessage, "Yes", "No");
                }
            }
            else
            {
                string warningMessage = "Your message is empty.\nDo you want to proceed anyway?";
                proceed = await Application.Current.MainPage.DisplayAlert("Warning", warningMessage, "Yes", "No");
            }

            return proceed;
        }

        private async void FillForm()
        {
            IsDetailsView = Training != null;
            Title = Training == null ? "Create new training" : Training.Name;
            if (formTraining != null)
            {
                FormTraining.Id = formTraining.Id;
                FormTraining.Name = formTraining.Name;
            }
            else if (Training != null)
            {
                FormTraining.Id = Training.Id;
                FormTraining.Name = Training.Name;
            }

            Members.Clear();
            if (members == null && IsDetailsView)
            {
                try
                {
                    var trainingDTO = await GetDataFromAPI<TrainingDTO>("Training/GetTrainingById", FormTraining.Id);
                    foreach(var participant in trainingDTO.ParticipantDTOs)
                    {
                        Members.Add(Models.Contact.MapDTOToModel(participant));
                    }
                    Date = trainingDTO.Date.Date;
                    Time = trainingDTO.Date.TimeOfDay;
                }
                catch
                {
                    return;
                }
            }
            else if (!IsDetailsView && members == null)
            {
                MessageTemplatesDropdown = await GetDataFromAPI<Dictionary<string, int>>("MessageTemplate/GetMessageTemplateDictionary");
                GroupsDropdown = await GetDataFromAPI<Dictionary<string, int>>("TrainingParticipant/GetTrainingGroupDictionary");
            }
            else if(members != null)
            {
                Members.Clear();
                foreach (var member in members)
                {
                    Members.Add(member);
                }
            }

        }

        private async void SelectTemplate()
        {
            string[] templateNames = MessageTemplatesDropdown.Keys.ToArray();
            string selectedTemplateName = await Application.Current.MainPage.DisplayActionSheet("Select template", "Cancel", null, templateNames);
            int selectedTemplateId;
            if(MessageTemplatesDropdown.TryGetValue(selectedTemplateName, out selectedTemplateId))
            {
                var selectedTemplate = await GetDataFromAPI<MessageTemplateDTO>("MessageTemplate/GetMessageTemplateById", selectedTemplateId);
                FormTraining.Message = selectedTemplate.Content;
            }
            else
            {
                //cancel selected
            }
        }

        private async void SelectGroup()
        {
            string[] groupNames = GroupsDropdown.Keys.ToArray();
            string selectedTemplate = await Application.Current.MainPage.DisplayActionSheet("Select template", "Cancel", null, groupNames);
            int selectedGroupId;
            if (GroupsDropdown.TryGetValue(selectedTemplate, out selectedGroupId))
            {
                var trainingGroupDTO = await GetDataFromAPI<TrainingGroupDTO>("TrainingParticipant/GetTrainingGroupById", selectedGroupId);
                foreach (var participantDTO in trainingGroupDTO.TrainingParticipantDTOs)
                {
                    Members.Add(Models.Contact.MapDTOToModel(participantDTO));
                }
            }
            else
            {

            }
        }
    }
}
