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
    [QueryProperty("formGroup", "formGroup")]
    public class TrainingGroupsViewModel
    {
        public TrainingGroup formGroup { get; set; }
        public TrainingGroupsViewModel()
        {
            Groups = new ObservableCollection<TrainingGroup>()
            {
                new TrainingGroup()
                {
                    Id = 1,
                    Name = "Group1",
                    MembersCount = 0
                },
                new TrainingGroup()
                {
                    Id = 2,
                    Name = "Group2",
                    MembersCount = 0
                },
                new TrainingGroup()
                {
                    Id = 3,
                    Name = "Group3",
                    MembersCount = 0
                },
            };
        }
        public ICommand EditCommand
        {
            get
            {
                return new Command<TrainingGroup>(EditGroup);
            }
        }
        public ICommand CreateCommand
        {
            get
            {
                return new Command(CreateGroup);
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new Command<TrainingGroup>(DeleteGroup);
            }
        }
        public ICommand AppearCommand
        {
            get
            {
                return new Command(Apperar);
            }
        }

        public ObservableCollection<TrainingGroup> Groups { get; }
        private async void EditGroup(TrainingGroup group)
        {
            var parameters = new Dictionary<string, object>
            {
                { "group", group }
            };
            await Shell.Current.GoToAsync("groupForm", parameters);
        }

        private async void DeleteGroup(TrainingGroup group)
        {
            bool delete = await Application.Current.MainPage.DisplayAlert("Are you sure?", string.Format("{0} will be deleted.", group.Name), "Yes", "No");
            if (delete)
            {
                //to do call do api
                Groups.Remove(group);
            }
        }

        private async void CreateGroup()
        {
            await Shell.Current.GoToAsync("groupForm");
        }

        private void Apperar()
        {
            if (formGroup == null)
            {
                return;
            }

            foreach (TrainingGroup group in Groups)
            {
                if (group.Id == formGroup.Id)
                {
                    group.Name = formGroup.Name;
                    group.MembersCount = formGroup.MembersCount;
                    return;
                }
            }
            Groups.Add(formGroup);
        }
    }
}
