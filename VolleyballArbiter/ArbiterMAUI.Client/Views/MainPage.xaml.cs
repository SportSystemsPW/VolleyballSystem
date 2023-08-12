﻿using CommunityToolkit.Mvvm.Input;

namespace ArbiterMAUI.Client.Views;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	public async void GoToMatchList(object sender, EventArgs e)
	{
        await Shell.Current.GoToAsync($"{nameof(MatchListPage)}", true);
    }
}

