
using ArbiterMAUI.Client.Models;
using ArbiterMAUI.Client.ViewModels;
using CommunityToolkit.Mvvm.Input;

namespace ArbiterMAUI.Client.Views
{
    public partial class MatchListPage : ContentPage
    {
        public MatchListPage(MatchListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
        }
    }
}