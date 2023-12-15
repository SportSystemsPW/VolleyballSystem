using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TreningOrganizer.MAUI.Models
{
    public class Contact : INotifyPropertyChanged
    {
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
