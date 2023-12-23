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
    public class Training : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private DateTime date;
        private int participantsPresent;
        private int participantsTotal;
        private string location;
        private double price;
        private string message;
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
        public DateTime Date
        {
            get
            {
                return date;
            }

            set
            {
                if (value != date)
                {
                    date = value;
                    NotifyPropertyChanged(nameof(DateString));
                }
            }
        }
        public int ParticipantsPresent
        {
            get
            {
                return participantsPresent;
            }

            set
            {
                if (value != participantsPresent)
                {
                    participantsPresent = value;
                    NotifyPropertyChanged(nameof(ParticipantsPresentToTotalString));
                }
            }
        }
        public int ParticipantsTotal
        {
            get
            {
                return participantsTotal;
            }

            set
            {
                if (value != participantsTotal)
                {
                    participantsTotal = value;
                    NotifyPropertyChanged(nameof(ParticipantsPresentToTotalString));
                }
            }
        }

        public string DateString
        {
            get
            {
                return date.ToString("d-MM-yyyy HH:mm");
            }
        }

        public string ParticipantsPresentToTotalString
        {
            get
            {
                return participantsPresent.ToString() + "/" + participantsTotal.ToString();
            }
        }

        public string Location
        {
            get
            {
                return location;
            }

            set
            {
                if (value != location)
                {
                    location = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double Price
        {
            get
            {
                return price;
            }

            set
            {
                if (value != price)
                {
                    price = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                if (value != message)
                {
                    message = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public static Training MapDTOToModel(TrainingDTO training)
        {
            return new Training
            {
                id = training.Id,
                name = training.Name,
                date = training.Date,
                location = training.Location, 
                price = training.Price,
                participantsTotal = training.ParticipantDTOs.Count,
                participantsPresent = training.ParticipantDTOs.Where(p => p.Presence).Count(),
                message = training.Message
            };
        }

        public static TrainingDTO MapModelToDTO(Training training, IEnumerable<Contact> contacts)
        {
            List<TrainingTrainingParticipantDTO> participantDTOs = new List<TrainingTrainingParticipantDTO>();
            foreach (var contact in contacts)
            {
                participantDTOs.Add(new TrainingTrainingParticipantDTO
                {
                    Name = contact.Name,
                    Phone = contact.Phone,
                    Id = contact.Id,
                    Presence = contact.Present
                });
            }
            return new TrainingDTO
            {
                Id = training.Id,
                Name = training.Name,
                Date = training.Date,
                Location = training.Location,
                Price = training.Price,
                ParticipantDTOs = participantDTOs,
                Message = training.Message
            };
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
