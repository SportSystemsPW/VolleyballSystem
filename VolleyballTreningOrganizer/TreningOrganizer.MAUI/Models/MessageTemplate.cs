using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Volleyball.DTO.TrainingOrganizer;

namespace TreningOrganizer.MAUI.Models
{
    public class MessageTemplate : INotifyPropertyChanged
    {
        private int id;
        private string content;
        private string templateName;
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                if (value != id)
                {
                    id = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Content
        {
            get
            {
                return content;
            }

            set
            {
                if (value != content)
                {
                    content = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string TemplateName
        {
            get
            {
                return templateName;
            }

            set
            {
                if (value != templateName)
                {
                    templateName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static MessageTemplate MapDTOToModel(MessageTemplateDTO messageTemplateDTO)
        {
            return new MessageTemplate
            {
                id = messageTemplateDTO.Id,
                content = messageTemplateDTO.Content,
                templateName = messageTemplateDTO.TemplateName
            };
        }

        public static MessageTemplateDTO MapModelToDTO(MessageTemplate messageTemplate)
        {
            return new MessageTemplateDTO
            {
                Id = messageTemplate.Id,
                Content = messageTemplate.Content,
                TemplateName = messageTemplate.TemplateName
            };
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
