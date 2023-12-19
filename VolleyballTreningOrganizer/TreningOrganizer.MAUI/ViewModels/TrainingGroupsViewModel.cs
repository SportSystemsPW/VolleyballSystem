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
    [QueryProperty("formGroup", "formGroup")]
    public class TrainingGroupsViewModel : BaseViewModel
    {
        public TrainingGroup formGroup { get; set; }
        public TrainingGroupsViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Groups = new ObservableCollection<TrainingGroup>();
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
                try
                {
                    await DeleteDataToAPI("TrainingParticipant/RemoveTrainingGroup", group.Id);
                }
                catch
                {
                    return;
                }
                Groups.Remove(group);
            }
        }

        private async void CreateGroup()
        {
            await Shell.Current.GoToAsync("groupForm");
        }

        private async void Apperar()
        {
            if (isInitialLoad)
            {
                try
                {
                    var trainingGroupsDTOs = await GetDataFromAPI<List<TrainingGroupDTO>>("TrainingParticipant/GetTrainingGroupsForTrainer");
                    foreach (var groupDTO in trainingGroupsDTOs)
                    {
                        Groups.Add(TrainingGroup.MapDTOToModel(groupDTO));
                    }
                    isInitialLoad = false;
                }
                catch
                {
                    return;
                }
            }

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
                    formGroup = null;
                    return;
                }
            }
            Groups.Add(new TrainingGroup
            {
                Id = formGroup.Id,
                Name = formGroup.Name,
                MembersCount = formGroup.MembersCount
            });
            formGroup = null;
        }
    }
}
