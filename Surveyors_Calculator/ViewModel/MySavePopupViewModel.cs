using System;
using System.Linq;
using System.Collections.Generic;
using Surveyors_Calculator.View;

namespace Surveyors_Calculator.ViewModel;

public partial class MySavePopupViewModel : BaseViewModel
{

    public ObservableCollection<string> CoordinateSystems { get; } = new();
    public ObservableCollection<string> Units { get; } = new();



    public MySavePopupViewModel()
    {
        Units.Add("metres");
        Units.Add("Cape feet");
        CoordinateSystems.Add("Lo 27°");
        CoordinateSystems.Add("Lo 29°");
        CoordinateSystems.Add("Lo 31°");
        CoordinateSystems.Add("Local");
    }


    [ObservableProperty]
    private string selectedCoordSys;
    [ObservableProperty]
    private string selectedUnits;
    [ObservableProperty]
    private string fileName;
    [ObservableProperty]
    private string missingData;

    // Reference to the View
    public MySavePopup Instance { get; set; }

    [RelayCommand]
    public void OkButton()
    {

        if (string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(SelectedCoordSys) || string.IsNullOrEmpty(SelectedUnits))
        {
            MissingData = "Fill all the fields";
        }
        else
        {
            FileData fileData = new FileData
            {
                fileName = FileName,
                coordSystem = SelectedCoordSys,
                units = SelectedUnits
            };
            Instance?.Close(fileData);
        }

    }


}
