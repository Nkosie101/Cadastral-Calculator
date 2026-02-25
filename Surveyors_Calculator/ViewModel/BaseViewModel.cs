//using CommunityToolkit.Mvvm.Input;
//using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;


namespace Surveyors_Calculator.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    public BaseViewModel()
    {

    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    string title;

    [ObservableProperty]
    bool isRefreshing;

    public bool IsNotBusy => !IsBusy;
}
