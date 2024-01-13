﻿using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TreningOrganizer.MAUI.Models;
using Communication = Microsoft.Maui.ApplicationModel.Communication;

namespace TreningOrganizer.MAUI.ViewModels
{
    [QueryProperty("formGroup", "formGroup")]
    [QueryProperty("group", "group")]
    [QueryProperty("members", "members")]
    [QueryProperty("Training", "Training")]
    [QueryProperty("fromTraining", "fromTraining")]
    public class ContactsViewModel
    {
        public ObservableCollection<Models.Contact> PhoneContacts { get; set; }
        public List<Models.Contact> members { get; set; }
        public TrainingGroup formGroup { get; set; }
        public TrainingGroup group { get; set; }
        public Training Training { get; set; }
        public Training formTraining { get; set; }
        private bool isInintialLoad = true;
        public ICommand AppearCommand
        {
            get
            {
                return new Command(Apperar);
            }
        }
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
        public ContactsViewModel()
        {
            PhoneContacts = new ObservableCollection<Models.Contact>();
        }
        private async void Apperar()
        {
            if (isInintialLoad)
            {
                var phoneContacts = await Communication.Contacts.GetAllAsync();
                foreach (var contact in phoneContacts)
                {
                    PhoneContacts.Add(new Models.Contact
                    {
                        Name = contact.DisplayName,
                        Phone = contact.Phones.FirstOrDefault().ToString(),
                        Selected = false
                    });
                }
                foreach (var contact in PhoneContacts)
                {
                    if (members.FirstOrDefault(m => m.Phone == contact.Phone) != null)
                    {
                        contact.Selected = true;
                    }
                }
                isInintialLoad = false;
            }   
        }
        private async void Cancel()
        {
            var parameters = new Dictionary<string, object>
            {
                { "group", group },
                { "formGroup", formGroup },
                { "members", members },
                { "Training", Training },
                { "formTraining", formTraining }
            };
            await Shell.Current.GoToAsync("..", parameters);
        }

        private async void Save()
        {
            List<Models.Contact> SelectedMembers = PhoneContacts.Where(m => m.Selected).ToList();
            var parameters = new Dictionary<string, object>
            {
                { "group", group },
                { "formGroup", formGroup },
                { "members", SelectedMembers },
                { "Training", Training },
                { "formTraining", formTraining }
            };
            await Shell.Current.GoToAsync("..", parameters);
        }
    }
}