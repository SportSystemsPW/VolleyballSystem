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
    [QueryProperty("group","group")]
    [QueryProperty("formGroup","formGroup")]
    [QueryProperty("members","members")]
    class TrainingGroupFormViewModel
    {
        public string Title { get; set; }
        public TrainingGroup group { get; set; }
        public TrainingGroup FormGroup { get; set; }
        public TrainingGroup formGroup { get; set; }
        public ObservableCollection<Models.Contact> Members { get; set; }
        public List<Models.Contact> members { get; set; }
        public TrainingGroupFormViewModel()
        {
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
            if (!validate)
            {
                //todo popup
            }
            //to do API call + refresh
            FormGroup.MembersCount = Members.Count;
            var parameters = new Dictionary<string, object>
            {
                { "formGroup", FormGroup }
            };
            await Shell.Current.GoToAsync("..", parameters);
        }

        private void FillForm()
        {
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

            if (members == null)
            {
                //pobrać członków z API
            }
            else
            {
                Members.Clear();
                foreach(var member in members)
                {
                    Members.Add(member);
                }
            }
            
        }
    }
}
