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
        IsLeastSquaresMethod = Preferences.Default.Get("TransformationMethod", false);
        IsWeightedTransform = Preferences.Default.Get("Weighting", false);
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    string title;

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    private bool isLeastSquaresMethod;

    [ObservableProperty]
    private bool isWeightedTransform;

    public bool IsNotBusy => !IsBusy;

    /*[ObservableProperty]
    bool is3D;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Is3D))]
    public bool is2D => !Is3D;*/
}
