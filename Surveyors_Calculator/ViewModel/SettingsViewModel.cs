using System;
using System.Linq;
using System.Collections.Generic;
/*using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;*/
using Surveyors_Calculator.View;
using Surveyors_Calculator.Services;
using Android.App;
//using SQLite;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;

//using Microsoft.Maui.ApplicationModel;


namespace Surveyors_Calculator.ViewModel;

public partial class SettingsViewModel : BaseViewModel
{
    /*public SetingsViewModel()
    {

    }*/

    [ObservableProperty]
    private bool isDarkMode;

    public SettingsViewModel()
    {
        // Load the saved preference (default to false/light)
        IsDarkMode = Preferences.Default.Get("AppThemeDark", false);
        isDarkMode = Preferences.Default.Get("AppThemeDark", false);


    }

    partial void OnIsDarkModeChanged(bool value)
    {
        // 1. Save the preference
        Preferences.Default.Set("AppThemeDark", value);

        // 2. Apply the theme immediately
        Microsoft.Maui.Controls.Application.Current.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;

    }

    /*[RelayCommand]
    async Task BackButton()
    {
        await AppShell.Current.GoToAsync("..");
    }*/

}
