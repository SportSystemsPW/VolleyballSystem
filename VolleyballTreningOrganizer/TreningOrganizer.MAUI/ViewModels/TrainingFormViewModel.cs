using Javax.Xml.Transform;
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
    [QueryProperty("Training", "Training")]
    [QueryProperty("formTraining", "formTraining")]
    [QueryProperty("members", "members")]
    public class TrainingFormViewModel
    {
        //training form and training details was supposed to be single view but binding to IsVisible and IsEnabled doesn't work
        public string Title { get; set; }
        public Training Training { get; set; }
        public Training FormTraining { get; set; }
        public Training formTraining { get; set; }
        public ObservableCollection<Models.Contact> Members { get; set; }
        public List<Models.Contact> members { get; set; }
        public bool IsDetailsView { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public TimeSpan Time { get; set; } = DateTime.Now.TimeOfDay;
        private Dictionary<string, int> MessageTemplatesDropdown { get; set; }
        private Dictionary<string, int> GroupsDropdown { get; set; }
        public TrainingFormViewModel()
        {
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
                //todo call api to update present list
                FormTraining.ParticipantsPresent = presentMembers.Count();
            }
            else
            {
                bool validate = true;
                if (!validate)
                {
                    //todo popup
                }
                //to do API call + refresh
                FormTraining.ParticipantsPresent = 0;
                FormTraining.Date = Date.Date + Time;
            }
            FormTraining.ParticipantsTotal = Members.Count;
            var parameters = new Dictionary<string, object>
            {
                { "FormTraining", FormTraining }
            };
            await Shell.Current.GoToAsync("..", parameters);
        }

        private void FillForm()
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

            if (members == null)
            {
                //pobrać członków z API
                Members.Add(new Models.Contact
                {
                    Name = "Antek",
                    Phone = "123123123",
                    Present = true
                });
                Members.Add(new Models.Contact
                {
                    Name = "Antek2",
                    Phone = "123123123",
                    Present = false
                });
            }
            else
            {
                Members.Clear();
                foreach (var member in members)
                {
                    Members.Add(member);
                }
            }

            if (IsDetailsView)
            {
                //pobrać uczestników zraz z obecnością
            }
            else
            {
                //pobrać dropdowny grup i templatek
                MessageTemplatesDropdown = new Dictionary<string, int>()
                {
                    { "Template1", 1 },
                    { "Template2", 2 },
                    { "Template3", 3 },
                };
                GroupsDropdown = new Dictionary<string, int>()
                {
                    { "Group1", 1 },
                    { "Group2", 2 },
                    { "Group3", 3 },
                };
            }

        }

        private async void SelectTemplate()
        {
            string[] templateNames = MessageTemplatesDropdown.Keys.ToArray();
            string selectedTemplate = await Application.Current.MainPage.DisplayActionSheet("Select template", "Cancel", null, templateNames);
            int selectedTemplateId;
            if(MessageTemplatesDropdown.TryGetValue(selectedTemplate, out selectedTemplateId))
            {
                //template selected
                //todo download content
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

            }
            else
            {

            }
        }
    }
}
