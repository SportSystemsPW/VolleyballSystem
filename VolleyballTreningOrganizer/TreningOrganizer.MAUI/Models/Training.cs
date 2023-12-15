using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TreningOrganizer.MAUI.Models
{
    public class Training : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private DateTime date;
        private int participantsPresent;
        private int participantsTotal;
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

        public string Location { get; set; }
        public double Price { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
