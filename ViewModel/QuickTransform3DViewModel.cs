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

            //AppShell.Current.DisplayAlert("Error", $"{ToCoords.Count}", "OK");
            Coordinate point1 = new Coordinate
            {
                name = "p1",
                x = Convert.ToDouble(e1Entry),
                y = Convert.ToDouble(n1Entry),
                z = Convert.ToDouble(z1Entry),

            };
            ToCoords.Add(point1);

            Coordinate point2 = new Coordinate
            {
                name = "p2",
                x = Convert.ToDouble(e2Entry),
                y = Convert.ToDouble(n2Entry),
                z = Convert.ToDouble(z2Entry),

            };
            ToCoords.Add(point2);
            Coordinate point3 = new Coordinate
            {
                name = "p3",
                x = Convert.ToDouble(e3Entry),
                y = Convert.ToDouble(n3Entry),
                z = Convert.ToDouble(z3Entry),

            };
            ToCoords.Add(point3);
            Coordinate point4 = new Coordinate
            {
                name = "p4",
                x = Convert.ToDouble(e4Entry),
                y = Convert.ToDouble(n4Entry),
                z = Convert.ToDouble(z4Entry),

            };
            FromCoords.Add(point4);
            Coordinate point5 = new Coordinate
            {
                name = "p5",
                x = Convert.ToDouble(e5Entry),
                y = Convert.ToDouble(n5Entry),
                z = Convert.ToDouble(z5Entry),

            };
            FromCoords.Add(point5);
            Coordinate point6 = new Coordinate
            {
                name = "p6",
                x = Convert.ToDouble(e6Entry),
                y = Convert.ToDouble(n6Entry),
                z = Convert.ToDouble(z6Entry),

            };
            FromCoords.Add(point6);

            Coordinate point7 = new Coordinate
            {
                name = "p7",
                x = Convert.ToDouble(e7Entry),
                y = Convert.ToDouble(n7Entry),
                z = Convert.ToDouble(z7Entry),

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
            var result = formulae.Transform3DQuick(E7Entry, N7Entry, Z7Entry);
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
