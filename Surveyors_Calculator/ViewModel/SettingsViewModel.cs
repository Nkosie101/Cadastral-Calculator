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
    [ObservableProperty]
    private bool isLeastSquares;
    [ObservableProperty]
    private bool isWeighted;
    [ObservableProperty]
    private bool isSVD;

    public SettingsViewModel()
    {
        // Load the saved preference (default to false/light)
        IsDarkMode = Preferences.Default.Get("AppThemeDark", false);
        isDarkMode = Preferences.Default.Get("AppThemeDark", false);
        IsLeastSquaresMethod = Preferences.Default.Get("TransformationMethod", false);
        IsWeightedTransform = Preferences.Default.Get("Weighting", false);
        //IsLeastSquares = Preferences.Default.Get("TransformationMethod", false);
        //IsWeighted = Preferences.Default.Get("Weighting", false);
        SetRadio();

    }

    partial void OnIsDarkModeChanged(bool value)
    {
        // Saving the preference
        Preferences.Default.Set("AppThemeDark", value);

        Microsoft.Maui.Controls.Application.Current.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;

    }

    void SetRadio()
    {
        if (IsLeastSquaresMethod)
        {
            IsLeastSquares = true;
        }
        else if (IsWeightedTransform)
        {
            IsWeighted = true;
        }
        else
        {
            IsSVD = true;
        }
    }

    partial void OnIsLeastSquaresChanged(bool value)
    {
        Preferences.Default.Set("TransformationMethod", value);
        IsLeastSquaresMethod = value;

    }

    partial void OnIsWeightedChanged(bool value)
    {

        Preferences.Default.Set("Weighting", value);
        IsWeightedTransform = value;
    }



    /*[RelayCommand]
    async Task BackButton()
    {
        await AppShell.Current.GoToAsync("..");
    }*/

}
