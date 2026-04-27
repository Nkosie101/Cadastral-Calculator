using System;
using System.Linq;
using System.Collections.Generic;
//using CommunityToolkit.Mvvm.ComponentModel;
//using System.Threading.Tasks;
using Surveyors_Calculator.View;
//using Surveyors_Calculator.Model;


namespace Surveyors_Calculator.ViewModel;


public partial class ConversionsViewModel : BaseViewModel
{
    public ObservableCollection<string> ConversionUnitsFrom { get; } = new();
    public ObservableCollection<string> ConversionUnitsTo { get; } = new();
    public ObservableCollection<string> ConversionTypes { get; } = new();
    public ObservableCollection<string> UnitSystems { get; } = new();

    public ConversionsViewModel()
    {
        //formulae = new Formulae { name = "Join", image = "join_image.avif" };
        //Formulas.Add(formulae);
        ConversionTypes = new ObservableCollection<string>();
        ConversionUnitsFrom = new ObservableCollection<string>();
        ConversionUnitsTo = new ObservableCollection<string>();
        UnitSystems = new ObservableCollection<string>();
        //GetConversion();

        UnitSystems.Add("Metric");
        UnitSystems.Add("Cape/English");

        ConversionTypes.Add("Length");
        ConversionTypes.Add("Area");

    }




    [ObservableProperty]
    private double input = 0;
    [ObservableProperty]
    private string output = string.Empty;
    [ObservableProperty]
    private string selectedUnit1 = string.Empty;
    [ObservableProperty]
    private string selectedUnit2 = string.Empty;
    [ObservableProperty]
    private string selectedType = string.Empty;
    [ObservableProperty]
    private string unitSystem = "Metric";
    [ObservableProperty]
    private double factor = 0;

    partial void OnSelectedTypeChanged(string value)
    {
        ConversionUnitsFrom.Clear();
        ConversionUnitsTo.Clear();
        SelectedUnit1 = string.Empty;
        SelectedUnit2 = string.Empty;
        FillUnitsCommand.Execute(null);

    }

    partial void OnUnitSystemChanged(string value)
    {
        ConversionUnitsFrom.Clear();
        ConversionUnitsTo.Clear();
        SelectedUnit1 = string.Empty;
        SelectedUnit2 = string.Empty;
        FillUnitsCommand.Execute(null);
    }

    [RelayCommand]
    async Task FillUnits()
    {
        if (SelectedType == "Area")
        {
            if (UnitSystem == "Metric")
            {
                ConversionUnitsFrom.Add("English Sq. feet");
                ConversionUnitsFrom.Add("Cape Sq. feet");
                ConversionUnitsFrom.Add("Acres");
                ConversionUnitsFrom.Add("Cape Sq. roods");
                ConversionUnitsFrom.Add("Morgen");
                ConversionUnitsFrom.Add("Sq. roods");
                ConversionUnitsFrom.Add("Hectares");
                ConversionUnitsFrom.Add("Sq. Metres");

                ConversionUnitsTo.Add("Sq. Metres");
                ConversionUnitsTo.Add("Hectares");
                ConversionUnitsTo.Add("Acres");


            }
            else
            {
                ConversionUnitsTo.Add("English Sq. feet");
                ConversionUnitsTo.Add("Cape Sq. feet");
                ConversionUnitsTo.Add("Acres");
                ConversionUnitsTo.Add("Cape Sq. roods");
                ConversionUnitsTo.Add("Morgen");
                //ConversionUnitsTo.Add("Sq. roods");

                ConversionUnitsFrom.Add("Sq. Metres");
                ConversionUnitsFrom.Add("Hectares");
                ConversionUnitsFrom.Add("Acres");
            }


        }
        else if (SelectedType == "Length")
        {
            //AppShell.Current.DisplayAlert("Error", $"{}", "OK");
            if (UnitSystem == "Metric")
            {
                ConversionUnitsFrom.Add("English feet");
                ConversionUnitsFrom.Add("Cape feet");
                ConversionUnitsFrom.Add("Cape roods");
                ConversionUnitsTo.Add("Metres");
            }
            else
            {
                ConversionUnitsTo.Add("English feet");
                ConversionUnitsTo.Add("Cape feet");
                ConversionUnitsTo.Add("Cape roods");
                ConversionUnitsFrom.Add("Metres");
            }


        }
    }




    [RelayCommand]
    async Task GetConversion()
    {
        try
        {
            Factor = 0;
            if (selectedType == "Length" && UnitSystem == "Cape/English")
            {
                //AppShell.Current.DisplayAlert("Error", $"{SelectedUnit1}", "OK");
                switch (SelectedUnit2)
                {
                    case "Cape feet":
                        Factor = 3.17605937;
                        break;
                    case "English feet":
                        Factor = 3.28084558;
                        break;
                    case "Cape roods":
                        Factor = 0.264671614;
                        break;
                    default:
                        Factor = 1;
                        break;
                }
                //AppShell.Current.DisplayAlert("Error", $"{Factor}", "OK");
            }
            else if (selectedType == "Length" && UnitSystem == "Metric")
            {
                switch (SelectedUnit1)
                {
                    case "Cape feet":
                        Factor = 0.304799472;
                        break;
                    case "English feet":
                        Factor = 0.314855575; break;
                    case "Cape roods":
                        Factor = 3.7782669; break;
                    default:
                        Factor = 1;
                        break;
                }
            }

            if (selectedType == "Area" && UnitSystem == "Metric")
            {
                switch (SelectedUnit1, SelectedUnit2)
                {
                    case ("English Sq. feet", "Sq. Metres"):
                        Factor = 0.092902718;
                        break;
                    case ("English Sq. feet", "Hectares"):
                        Factor = 0.0000092902718;
                        break;
                    case ("Acres", "Sq. Metres"):
                        Factor = 4046.8424;
                        break;
                    case ("Acres", "Hectares"):
                        Factor = 0.40468424;
                        break;
                    case ("Cape Sq. feet", "Sq. Metres"):
                        Factor = 0.0991340332;
                        break;
                    case ("Cape Sq. feet", "Hectares"):
                        Factor = 0.00000991340332;
                        break;
                    case ("Cape Sq. roods", "Sq. Metres"):
                        Factor = 14.2753008;
                        break;
                    case ("Cape Sq. roods", "Hectares"):
                        Factor = 0.00142753008;
                        break;
                    case ("Morgen", "Sq. Metres"):
                        Factor = 8565.18047;
                        break;
                    case ("Morgen", "Hectares"):
                        Factor = 0.856518047;
                        break;
                    case ("Morgen", "Acres"):
                        Factor = 2.116509522;
                        break;
                    case ("Sq. roods", "Acres"):
                        Factor = 0.00352751587;
                        break;
                    case ("Hectares", "Sq. Metres"):
                        Factor = 10000;
                        break;
                    case ("Hectares", "Acres"):
                        Factor = 2.47106238;
                        break;
                    case ("Sq. Metres", "Hectares"):
                        Factor = 0.0001;
                        break;
                    case ("Sq. Metres", "Acres"):
                        Factor = 0.000247106238;
                        break;
                    default:
                        Factor = 1;
                        break;

                }
            }
            else if (selectedType == "Area" && UnitSystem == "Cape/English")
            {
                switch (SelectedUnit1, SelectedUnit2)
                {
                    case ("Sq. Metres", "English Sq. feet"):
                        Factor = 10.7639477;
                        break;
                    case ("Sq. Metres", "Acres"):
                        Factor = 0.000247106238;
                        break;
                    case ("Hectares", "English Sq. feet"):
                        Factor = 107639.477;
                        break;
                    case ("Hectares", "Acres"):
                        Factor = 2.47106238;
                        break;
                    case ("Sq. Metres", "Cape Sq. feet"):
                        Factor = 10.0873531;
                        break;
                    case ("Sq. Metres", "Cape Sq. roods"):
                        Factor = 0.0700510634;
                        break;
                    case ("Sq. Metres", "Morgen"):
                        Factor = 0.000116751772;
                        break;
                    case ("Hectares", "Cape Sq. feet"):
                        Factor = 100873.531;
                        break;
                    case ("Hectares", "Cape Sq. roods"):
                        Factor = 700.510634;
                        break;
                    case ("Hectares", "Morgen"):
                        Factor = 1.16751772;
                        break;
                    default:
                        Factor = 1;
                        break;

                }
                //AppShell.Current.DisplayAlert("Error", $"{Factor}", "OK");



            }
            Output = Convert.ToString(Input * Factor);
        }
        catch (Exception e)
        {
            await AppShell.Current.DisplayAlert("Error", $"Check your entries! {e.Message}", "OK");
        }
        finally
        {

        }
    }

}




