using ArbiterMAUI.Client.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbiterMAUI.Client.ViewModels
{
    public partial class FilterPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        DateTime startDate;

        [ObservableProperty]
        DateTime endDate;

        [RelayCommand]
        [Obsolete]
        async Task SaveFilters()
        {
            App._fromDateSearch = StartDate;
            App._toDateSearch = EndDate;

            MessagingCenter.Send(new FiltersChangedMessage(), "ChangedSuccessfully");
            await Shell.Current.GoToAsync("..");
        }

        public FilterPageViewModel()
        {
            StartDate = App._fromDateSearch.Value;
            EndDate = App._toDateSearch.Value;
        }
    }
}
