using System;
using System.Linq;
using System.Collections.Generic;
using Surveyors_Calculator.View;

namespace Surveyors_Calculator.ViewModel;

public partial class MyCSVMapPopupViewModel : BaseViewModel
{

    public ObservableCollection<string> CoordinateSystems { get; } = new();
    public ObservableCollection<string> Units { get; } = new();



    public MyCSVMapPopupViewModel()
    {
        Units.Add("metres");
        Units.Add("Cape feet");
        CoordinateSystems.Add("Lo 27°");
        CoordinateSystems.Add("Lo 29°");
        CoordinateSystems.Add("Lo 31°");
        CoordinateSystems.Add("Local");
    }


    [ObservableProperty]
    private string yColumn = string.Empty;
    [ObservableProperty]
    private string xColumn = string.Empty;
    [ObservableProperty]
    private string zColumn = string.Empty;
    [ObservableProperty]
    private string nameColumn;
    [ObservableProperty]
    private string missingData;

    // Reference to the View
    public MyCSVMapPopup Instance { get; set; }

    [RelayCommand]
    public void OkButton()
    {

        if (string.IsNullOrEmpty(NameColumn) || string.IsNullOrEmpty(YColumn) || string.IsNullOrEmpty(XColumn) || string.IsNullOrEmpty(ZColumn))
        {
            MissingData = "Fill all the fields";
        }
        else
        {
            /*FileData fileData = new FileData
            {
                fileName = FileName,
                coordSystem = SelectedCoordSys,
                units = SelectedUnits
            };*/
            List<string> columnData = [NameColumn, YColumn, XColumn, ZColumn];
            Instance?.Close(columnData);
        }

    }


}
