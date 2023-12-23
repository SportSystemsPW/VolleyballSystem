using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volleyball.DTO.TrainingOrganizer;

namespace TreningOrganizer.MAUI.Models
{
    public class Contact : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private bool selected;
        public bool Selected
        {
            get
            {
                return selected;
            }

            set
            {
                if (value != selected)
                {
                    selected = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Phone { get; set; }

        private bool present;
        public bool Present
        {
            get
            {
                return present;
            }

            set
            {
                if (value != present)
                {
                    present = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static Contact MapDTOToModel(TrainingParticipantDTO trainingParticipantDTO)
        {
            return new Contact
            {
                Name = trainingParticipantDTO.Name,
                Phone = trainingParticipantDTO.Phone,
                Id = trainingParticipantDTO.Id
            };
        }

        public static Contact MapDTOToModel(TrainingTrainingParticipantDTO trainingParticipantDTO)
        {
            return new Contact
            {
                Name = trainingParticipantDTO.Name,
                Phone = trainingParticipantDTO.Phone,
                Id = trainingParticipantDTO.Id,
                present = trainingParticipantDTO.Presence
            };
        }

        public static TrainingParticipantDTO MapModelToDTO(Contact contact)
        {
            return new TrainingParticipantDTO
            {
                Name = contact.Name,
                Phone = contact.Phone,
                Id = contact.Id
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
