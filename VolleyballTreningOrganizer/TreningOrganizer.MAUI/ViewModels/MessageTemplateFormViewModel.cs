using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TreningOrganizer.MAUI.Models;

namespace TreningOrganizer.MAUI.ViewModels
{
    [QueryProperty("Template", "Template")]
    public class MessageTemplateFormViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public MessageTemplate Template { get; set; }
        public MessageTemplate FormTemplate { get; set; } = new MessageTemplate();
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
            bool validate = true;
            if (!validate)
            {
                //todo popup
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
            Title = Template == null ? "Create new message template" : "Edit message template";
            Template = Template ?? new MessageTemplate();
            FormTemplate.TemplateName = Template.TemplateName;
            FormTemplate.Content = Template.Content;
            FormTemplate.Id = Template.Id;
        }
    }
}
