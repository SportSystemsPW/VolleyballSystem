using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TreningOrganizer.MAUI.Models;
using Volleyball.DTO.TrainingOrganizer;

namespace TreningOrganizer.MAUI.ViewModels
{
    [QueryProperty("formTemplate","formTemplate")]
    public class MessageTemplatesViewModel : BaseViewModel
    {
        public MessageTemplate formTemplate { get; set; }
        public ICommand EditCommand
        {
            get
            {
                return new Command<MessageTemplate>(EditTemplate);
            }
        }
        public ICommand CreateCommand
        {
            get
            {
                return new Command(CreateTemplate);
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new Command<MessageTemplate>(DeleteTemplate);
            }
        }
        public ICommand AppearCommand
        {
            get
            {
                return new Command(Apperar);
            }
        }

        public ObservableCollection<MessageTemplate> Templates { get; }
        public MessageTemplatesViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Templates = new ObservableCollection<MessageTemplate>();
        }
        private async void EditTemplate(MessageTemplate template)
        {
            var parameters = new Dictionary<string, object>
            {
                { "Template", template }
            };
            await Shell.Current.GoToAsync("templateForm", parameters);
        }

        private async void DeleteTemplate(MessageTemplate template)
        {
            bool delete = await Application.Current.MainPage.DisplayAlert("Are you sure?", string.Format("{0} will be deleted.", template.TemplateName),"Yes", "No");
            if (delete)
            {
                try
                {
                    await DeleteDataToAPI("MessageTemplate/RemoveMessageTemplate", template.Id);
                }
                catch
                {
                    return;
                }
                Templates.Remove(template);
            }
        }

        private async void CreateTemplate()
        {
            await Shell.Current.GoToAsync("templateForm");
        }

        private async void Apperar()
        {
            if (isInitialLoad)
            {
                try
                {
                    var templateDTOs = await GetDataFromAPI<List<MessageTemplateDTO>>("MessageTemplate/GetMessageTemplatesForTrainer");
                    foreach (var templateDTO in templateDTOs)
                    {
                        Templates.Add(MessageTemplate.MapDTOToModel(templateDTO));
                    }
                    isInitialLoad = false;
                }
                catch
                {
                    return;
                }
            }
            if (formTemplate == null)
            {
                return;
            }

            foreach(MessageTemplate template in Templates)
            {
                if(template.Id == formTemplate.Id)
                {
                    template.Content = formTemplate.Content;
                    template.TemplateName = formTemplate.TemplateName;
                    formTemplate = null;
                    return;
                }
            }
            Templates.Add(new MessageTemplate
            {
                Id = formTemplate.Id,
                Content = formTemplate.Content,
                TemplateName = formTemplate.TemplateName
            });
            formTemplate = null;
        }

    }
}
