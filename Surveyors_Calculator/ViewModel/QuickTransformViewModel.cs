using System;
using System.Linq;
using System.Collections.Generic;
/*using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;*/
using Surveyors_Calculator.View;

namespace Surveyors_Calculator.ViewModel;

public partial class QuickTransformViewModel : BaseViewModel
{
    public QuickTransformViewModel()
    {

    }

    Formulae formulae = new Formulae();

    [ObservableProperty]
    private string e1Entry = string.Empty;
    [ObservableProperty]
    private string n1Entry = string.Empty;
    [ObservableProperty]
    private string e2Entry = string.Empty;
    [ObservableProperty]
    private string n2Entry = string.Empty;
    [ObservableProperty]
    private string e3Entry = string.Empty;
    [ObservableProperty]
    private string n3Entry = string.Empty;
    [ObservableProperty]
    private string e4Entry = string.Empty;
    [ObservableProperty]
    private string n4Entry = string.Empty;
    [ObservableProperty]
    private string e5Entry = string.Empty;
    [ObservableProperty]
    private string n5Entry = string.Empty;

    [ObservableProperty]
    private string easting = "0";
    [ObservableProperty]
    private string northing = "0";





    [RelayCommand]
    async Task GetTransformedCoordinates()
    {
        if (IsBusy)
            return;


        try
        {
            IsBusy = true;
            var scaleFactor = formulae.GetScaleFactor(E1Entry, N1Entry, E2Entry, N2Entry, E3Entry, N3Entry, E4Entry, N4Entry);
            //Easting = scaleFactor.ToString();
            var rotation = formulae.GetRotation(E1Entry, N1Entry, E2Entry, N2Entry, E3Entry, N3Entry, E4Entry, N4Entry);
            formulae.GetTranslation(scaleFactor, rotation, E1Entry, N1Entry, E3Entry, N3Entry);

            var result = formulae.Transform(E5Entry, N5Entry);
            Easting = result.Easting.ToString();
            Northing = result.Northing.ToString();

            await AppShell.Current.DisplayAlert("Result", $"E: {result.Easting}, N: {result.Northing}", "OK");
        }
        catch (Exception e)
        {
            await AppShell.Current.DisplayAlert("Error", $"Check your entries! {e.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
