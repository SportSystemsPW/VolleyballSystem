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
    [QueryProperty("FormTemplate","FormTemplate")]
    class MessageTemplatesViewModel
    {
        public MessageTemplate FormTemplate { get; set; }
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
        public MessageTemplatesViewModel()
        {
            Templates = new ObservableCollection<MessageTemplate>
            {
                new MessageTemplate()
                {
                    Id = 1,
                    TemplateName = "Test",
                    Content = "Test content",
                },
                new MessageTemplate()
                {
                    Id = 2,
                    TemplateName = "Test2",
                    Content = "Test content2",
                },
                new MessageTemplate()
                {
                    Id = 3,
                    TemplateName = "Test3",
                    Content = "Test content3",
                }
            };
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
                //to do call do api
                Templates.Remove(template);
            }
        }

        private async void CreateTemplate()
        {
            await Shell.Current.GoToAsync("templateForm");
        }

        private void Apperar()
        {
            if(FormTemplate == null)
            {
                return;
            }

            foreach(MessageTemplate template in Templates)
            {
                if(template.Id == FormTemplate.Id)
                {
                    template.Content = FormTemplate.Content;
                    template.TemplateName = FormTemplate.TemplateName;
                    return;
                }
            }
            Templates.Add(FormTemplate);
        }

    }
}
