﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TreningOrganizer.MAUI.Models
{
    public class TrainingParticipant
    {
        private int id;
        private string name;
        private string phone;
        private double balance;

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
        public string Phone
        {
            get
            {
                return phone;
            }

            set
            {
                if (value != phone)
                {
                    phone = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double Balance
        {
            get
            {
                return balance;
            }

            set
            {
                if (value != balance)
                {
                    balance = value;
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
