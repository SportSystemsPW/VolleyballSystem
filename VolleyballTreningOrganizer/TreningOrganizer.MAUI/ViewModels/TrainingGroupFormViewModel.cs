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
    [QueryProperty("group","group")]
    [QueryProperty("formGroup","formGroup")]
    [QueryProperty("members","members")]
    public class TrainingGroupFormViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public TrainingGroup group { get; set; }
        public TrainingGroup FormGroup { get; set; }
        public TrainingGroup formGroup { get; set; }
        public ObservableCollection<Models.Contact> Members { get; set; }
        public List<Models.Contact> members { get; set; }
        public int NameMaxLength { get; } = 50;
        private bool isEdit;
        public TrainingGroupFormViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Members = new ObservableCollection<Models.Contact>();
            FormGroup = new TrainingGroup();
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
                { "group", group },
                { "formGroup", FormGroup },
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
            bool validate = true;
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(FormGroup.Name))
            {
                validate = false;
                errors.Add("Group name can't be empty");
            }
            else if (FormGroup.Name.Length > NameMaxLength)
            {
                validate = false;
                errors.Add($"Message template name can't be longer than {NameMaxLength} characters");
            }
            if (Members.Count == 0)
            {
                validate = false;
                errors.Add("Group must have at least one member");
            }
            
            if (validate)
            {
                try
                {
                    if (isEdit)
                    {
                        await PutRequest("TrainingParticipant/EditTrainingGroup", TrainingGroup.MapModelToDTO(FormGroup, Members));
                    }
                    else
                    {
                        int id = await PostRequest<int>("TrainingParticipant/CreateTrainingGroup", TrainingGroup.MapModelToDTO(FormGroup, Members));
                        FormGroup.Id = id;
                    }
                }
                catch
                {
                    await Shell.Current.GoToAsync("..");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", string.Join('\n', errors), "OK");
                return;
            }
            FormGroup.MembersCount = Members.Count;
            var parameters = new Dictionary<string, object>
            {
                { "formGroup", FormGroup }
            };
            await Shell.Current.GoToAsync("..", parameters);
        }

        private async void FillForm()
        {
            isEdit = group != null;
            Title = group == null ? "Create new training group" : "Edit training group";
            if (formGroup != null)
            {
                FormGroup.Id = formGroup.Id;
                FormGroup.Name = formGroup.Name;
            }
            else if(group != null)
            {
                FormGroup.Id = group.Id;
                FormGroup.Name = group.Name;
            }

            Members.Clear();
            //cannot use isInitialLoad - only true when entering edit for first time
            if (members == null && isEdit)
            {
                try
                {
                    var trainingGroupDTO = await GetRequest<TrainingGroupDTO>("TrainingParticipant/GetTrainingGroupById", group.Id);
                    foreach (var participantDTO in trainingGroupDTO.TrainingParticipantDTOs)
                    {
                        Members.Add(Models.Contact.MapDTOToModel(participantDTO));
                    }
                }
                catch
                {
                    return;
                }
            }
            else if (members != null)
            {
                foreach(var member in members)
                {
                    Members.Add(member);
                }
            }
            
        }
    }
}
