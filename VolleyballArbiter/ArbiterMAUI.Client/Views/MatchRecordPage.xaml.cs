using ArbiterMAUI.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbiterMAUI.Client.Views
{
	public partial class MatchRecordPage : ContentPage
	{
		public MatchRecordPage (MatchRecordViewModel viewModel)
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