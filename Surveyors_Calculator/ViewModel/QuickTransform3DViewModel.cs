using System;
using System.Linq;
using System.Collections.Generic;
/*using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;*/
using Surveyors_Calculator.View;

namespace Surveyors_Calculator.ViewModel;

public partial class QuickTransform3DViewModel : BaseViewModel
{
    public QuickTransform3DViewModel()
    {

    }

    Formulae formulae = new Formulae();

    [ObservableProperty]
    private string e1Entry = string.Empty;
    [ObservableProperty]
    private string n1Entry = string.Empty;
    [ObservableProperty]
    private string z1Entry = string.Empty;
    [ObservableProperty]
    private string e2Entry = string.Empty;
    [ObservableProperty]
    private string n2Entry = string.Empty;
    [ObservableProperty]
    private string z2Entry = string.Empty;
    [ObservableProperty]
    private string e3Entry = string.Empty;
    [ObservableProperty]
    private string n3Entry = string.Empty;
    [ObservableProperty]
    private string z3Entry = string.Empty;
    [ObservableProperty]
    private string e4Entry = string.Empty;
    [ObservableProperty]
    private string n4Entry = string.Empty;
    [ObservableProperty]
    private string z4Entry = string.Empty;
    [ObservableProperty]
    private string e5Entry = string.Empty;
    [ObservableProperty]
    private string n5Entry = string.Empty;
    [ObservableProperty]
    private string z5Entry = string.Empty;
    [ObservableProperty]
    private string e6Entry = string.Empty;
    [ObservableProperty]
    private string n6Entry = string.Empty;
    [ObservableProperty]
    private string z6Entry = string.Empty;
    [ObservableProperty]
    private string e7Entry = string.Empty;
    [ObservableProperty]
    private string n7Entry = string.Empty;
    [ObservableProperty]
    private string z7Entry = string.Empty;

    [ObservableProperty]
    private string easting = "0";
    [ObservableProperty]
    private string northing = "0";
    [ObservableProperty]
    private string elevation = "0";




    [RelayCommand]
    async Task Get3DTransformedCoordinates()
    {
        if (IsBusy)
            return;


        try
        {
            IsBusy = true;
            //var scaleFactor = formulae.GetScaleFactor(e1Entry, n1Entry, e2Entry, n2Entry, e3Entry, n3Entry, e4Entry, n4Entry);
            //Easting = scaleFactor.ToString();
            //var rotation = formulae.GetRotation(e1Entry, n1Entry, e2Entry, n2Entry, e3Entry, n3Entry, e4Entry, n4Entry);
            //formulae.GetTranslation(scaleFactor, rotation, e1Entry, n1Entry, e3Entry, n3Entry);
            formulae.createInitialMatrices(E1Entry, N1Entry, Z1Entry, E2Entry, N2Entry, Z2Entry, E3Entry, N3Entry, Z3Entry, E4Entry, N4Entry, Z4Entry, E5Entry, N5Entry, Z5Entry, E6Entry, N6Entry, Z6Entry);
            formulae.SVD();
            formulae.S3D();
            formulae.Translation();
            var result = formulae.Transform3D(E7Entry, N7Entry, Z7Entry);
            Easting = result.Easting.ToString();
            Northing = result.Northing.ToString();
            Elevation = result.Elevation.ToString();


            await AppShell.Current.DisplayAlert("Result", $"E: {result.Easting}, N: {result.Northing}, Z: {result.Elevation}", "OK");



            /*for (int i = 0; i < From.Count; i++)
            {

                e7Entry = Convert.ToString(From[i].y);
                n7Entry = Convert.ToString(From[i].x);
                z7Entry = Convert.ToString(From[i].z);

                var match = To.FirstOrDefault(s => s.name == From[i].name);

                if (match == null)
                {
                    var result = formulae.Transform3D(e7Entry, n7Entry, z7Entry);
                    //AppShell.Current.DisplayAlert("Error", $"{e7Entry}", "OK");
                    Coordinate toCoord = new Coordinate
                    {
                        name = Convert.ToString(From[i].name),
                        y = Convert.ToDouble(result.Easting),
                        x = Convert.ToDouble(result.Northing),
                        z = Convert.ToDouble(result.Elevation),
                        IsChecked = false
                    };
                    To.Add(toCoord);
                }
            }*/
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
