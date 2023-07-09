
using ArbiterMAUI.Client.ViewModels;

namespace ArbiterMAUI.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MatchListPage : ContentPage
    {
        public MatchListPage(MatchListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext= viewModel;
        }
    }
}