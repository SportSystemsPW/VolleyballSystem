using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Volleyball.DTO.TrainingOrganizer;

namespace TreningOrganizer.MAUI.Models
{
    public class TrainingGroup : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private int membersCount;
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
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int MembersCount
        {
            get
            {
                return membersCount;
            }
            set
            {
                if (value != membersCount)
                {
                    membersCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static TrainingGroup MapDTOToModel(TrainingGroupDTO trainingGroupDTO)
        {
            return new TrainingGroup
            {
                id = trainingGroupDTO.Id,
                name = trainingGroupDTO.Name,
                membersCount = trainingGroupDTO.MembersCount
            };
        }

        public static TrainingGroupDTO MapModelToDTO(TrainingGroup trainingGroup, IEnumerable<Contact> contacts)
        {
            List<TrainingParticipantDTO> participantDTOs = new List<TrainingParticipantDTO>();
            foreach(var contact in contacts)
            {
                participantDTOs.Add(Contact.MapModelToDTO(contact));
            }
            return new TrainingGroupDTO
            {
                Id = trainingGroup.Id,
                Name = trainingGroup.Name,
                TrainingParticipantDTOs = participantDTOs
            };
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
