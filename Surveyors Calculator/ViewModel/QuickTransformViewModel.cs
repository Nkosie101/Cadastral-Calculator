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
        FromCoords = new List<Coordinate>();
        ToCoords = new List<Coordinate>();
        ToTransform = new List<Coordinate>();
    }

    Formulae formulae = new Formulae();

    public List<Coordinate> FromCoords { get; } = new();
    public List<Coordinate> ToCoords { get; } = new();
    public List<Coordinate> ToTransform { get; } = new();

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

    [ObservableProperty]
    private bool is2D = true;
    [ObservableProperty]
    private bool is3D;





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

    partial void OnIs3DChanged(bool value)
    {
        Is2D = !Is3D;

    }

    [ObservableProperty]
    private string e1Entry3D = string.Empty;
    [ObservableProperty]
    private string n1Entry3D = string.Empty;
    [ObservableProperty]
    private string z1Entry3D = string.Empty;
    [ObservableProperty]
    private string e2Entry3D = string.Empty;
    [ObservableProperty]
    private string n2Entry3D = string.Empty;
    [ObservableProperty]
    private string z2Entry3D = string.Empty;
    [ObservableProperty]
    private string e3Entry3D = string.Empty;
    [ObservableProperty]
    private string n3Entry3D = string.Empty;
    [ObservableProperty]
    private string z3Entry3D = string.Empty;
    [ObservableProperty]
    private string e4Entry3D = string.Empty;
    [ObservableProperty]
    private string n4Entry3D = string.Empty;
    [ObservableProperty]
    private string z4Entry3D = string.Empty;
    [ObservableProperty]
    private string e5Entry3D = string.Empty;
    [ObservableProperty]
    private string n5Entry3D = string.Empty;
    [ObservableProperty]
    private string z5Entry3D = string.Empty;
    [ObservableProperty]
    private string e6Entry3D = string.Empty;
    [ObservableProperty]
    private string n6Entry3D = string.Empty;
    [ObservableProperty]
    private string z6Entry3D = string.Empty;
    [ObservableProperty]
    private string e7Entry3D = string.Empty;
    [ObservableProperty]
    private string n7Entry3D = string.Empty;
    [ObservableProperty]
    private string z7Entry3D = string.Empty;

    [ObservableProperty]
    private string easting3D = "0";
    [ObservableProperty]
    private string northing3D = "0";
    [ObservableProperty]
    private string elevation3D = "0";



    [RelayCommand]
    async Task Get3DTransformedCoordinates()
    {
        if (IsBusy)
            return;


        try
        {
            IsBusy = true;

            //AppShell.Current.DisplayAlert("Error", $"{ToCoords.Count}", "OK");
            Coordinate point1 = new Coordinate
            {
                name = "p1",
                x = Convert.ToDouble(e1Entry3D),
                y = Convert.ToDouble(n1Entry3D),
                z = Convert.ToDouble(z1Entry3D),

            };
            ToCoords.Add(point1);

            Coordinate point2 = new Coordinate
            {
                name = "p2",
                x = Convert.ToDouble(e2Entry3D),
                y = Convert.ToDouble(n2Entry3D),
                z = Convert.ToDouble(z2Entry3D),

            };
            ToCoords.Add(point2);
            Coordinate point3 = new Coordinate
            {
                name = "p3",
                x = Convert.ToDouble(e3Entry3D),
                y = Convert.ToDouble(n3Entry3D),
                z = Convert.ToDouble(z3Entry3D),

            };
            ToCoords.Add(point3);
            Coordinate point4 = new Coordinate
            {
                name = "p4",
                x = Convert.ToDouble(e4Entry3D),
                y = Convert.ToDouble(n4Entry3D),
                z = Convert.ToDouble(z4Entry3D),

            };
            FromCoords.Add(point4);
            Coordinate point5 = new Coordinate
            {
                name = "p5",
                x = Convert.ToDouble(e5Entry3D),
                y = Convert.ToDouble(n5Entry3D),
                z = Convert.ToDouble(z5Entry3D),

            };
            FromCoords.Add(point5);
            Coordinate point6 = new Coordinate
            {
                name = "p6",
                x = Convert.ToDouble(e6Entry3D),
                y = Convert.ToDouble(n6Entry3D),
                z = Convert.ToDouble(z6Entry3D),

            };
            FromCoords.Add(point6);

            Coordinate point7 = new Coordinate
            {
                name = "p7",
                x = Convert.ToDouble(e7Entry3D),
                y = Convert.ToDouble(n7Entry3D),
                z = Convert.ToDouble(z7Entry3D),

            };
            ToTransform.Add(point7);


            //var scaleFactor = formulae.GetScaleFactor(e1Entry, n1Entry, e2Entry, n2Entry, e3Entry, n3Entry, e4Entry, n4Entry);
            //Easting = scaleFactor.ToString();
            //var rotation = formulae.GetRotation(e1Entry, n1Entry, e2Entry, n2Entry, e3Entry, n3Entry, e4Entry, n4Entry);
            //formulae.GetTranslation(scaleFactor, rotation, e1Entry, n1Entry, e3Entry, n3Entry);
            formulae.createInitialMatrices(ToCoords, FromCoords, ToTransform);
            formulae.SVD();
            formulae.S3D();
            formulae.Translation();
            var result = formulae.Transform3DQuick(E7Entry3D, N7Entry3D, Z7Entry3D);
            Easting3D = result.Easting.ToString();
            Northing3D = result.Northing.ToString();
            Elevation3D = result.Elevation.ToString();


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

    [RelayCommand]
    async Task Clear()
    {
        E1Entry = string.Empty;
        N1Entry = string.Empty;
        E2Entry = string.Empty;
        N2Entry = string.Empty;
        E3Entry = string.Empty;
        N3Entry = string.Empty;
        E4Entry = string.Empty;
        N4Entry = string.Empty;
        E5Entry = string.Empty;
        N5Entry = string.Empty;


        E1Entry3D = string.Empty;
        N1Entry3D = string.Empty;
        Z1Entry3D = string.Empty;
        E2Entry3D = string.Empty;
        N2Entry3D = string.Empty;
        Z2Entry3D = string.Empty;
        E3Entry3D = string.Empty;
        N3Entry3D = string.Empty;
        Z3Entry3D = string.Empty;
        E4Entry3D = string.Empty;
        N4Entry3D = string.Empty;
        Z4Entry3D = string.Empty;
        E5Entry3D = string.Empty;
        N5Entry3D = string.Empty;
        Z5Entry3D = string.Empty;
        E6Entry3D = string.Empty;
        N6Entry3D = string.Empty;
        Z6Entry3D = string.Empty;
        E7Entry3D = string.Empty;
        N7Entry3D = string.Empty;
        Z7Entry3D = string.Empty;

        Easting3D = "0";
        Northing3D = "0";
        Elevation3D = "0";

        Easting = "0";
        Northing = "0";


    }
}

