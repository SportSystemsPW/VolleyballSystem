using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TreningOrganizer.MAUI.Models;
using Volleyball.DTO.TrainingOrganizer;

namespace TreningOrganizer.MAUI.ViewModels
{
    [QueryProperty("Template", "Template")]
    public class MessageTemplateFormViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public MessageTemplate Template { get; set; }
        public MessageTemplate FormTemplate { get; set; } = new MessageTemplate();
        public int NameMaxLength { get; } = 50;
        public int ContentMaxLength { get; } = 300;
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

        private bool isEdit;

        public MessageTemplateFormViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private async void Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void Save()
        {
            List<string> errors = new List<string>();
            bool validate = true;
            if (string.IsNullOrEmpty(FormTemplate.TemplateName))
            {
                validate = false;
                errors.Add("Message template name can't be empty");
            }
            else if(FormTemplate.TemplateName.Length > NameMaxLength)
            {
                validate = false;
                errors.Add($"Message template name can't be longer than {NameMaxLength} characters");
            }
            if (string.IsNullOrEmpty(FormTemplate.Content))
            {
                validate = false;
                errors.Add("Message template content can't be empty");
            }
            else if (FormTemplate.Content.Length > ContentMaxLength)
            {
                validate = false;
                errors.Add($"Message template content can't be longer than {ContentMaxLength} characters");
            }

            if (validate)
            {
                try
                {
                    if (isEdit)
                    {
                        await PutRequest("MessageTemplate/EditMessageTemplate", MessageTemplate.MapModelToDTO(FormTemplate));
                    }
                    else
                    {
                        int id = await PostRequest<int>("MessageTemplate/CreateMessageTemplate", MessageTemplate.MapModelToDTO(FormTemplate));
                        FormTemplate.Id = id;
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
            //to do API call + refresh
            var parameters = new Dictionary<string, object>
            {
                { "formTemplate", FormTemplate } //todo zwracac templatke zwrocona przez api
            };
            await Shell.Current.GoToAsync("..", parameters);
        }

        private void FillForm()
        {
            isEdit = Template != null;
            Title = Template == null ? "Create new message template" : "Edit message template";
            Template = Template ?? new MessageTemplate();
            FormTemplate.TemplateName = Template.TemplateName;
            FormTemplate.Content = Template.Content;
            FormTemplate.Id = Template.Id;
        }
    }
}
