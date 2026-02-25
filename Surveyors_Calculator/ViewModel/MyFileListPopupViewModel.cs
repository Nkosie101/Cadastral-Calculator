using System;
using System.Linq;
using System.Collections.Generic;
using Surveyors_Calculator.View;

namespace Surveyors_Calculator.ViewModel;
//[QueryProperty(nameof(tempCoordinates), "coordinatesKey")]
public partial class MyFileListPopupViewModel : BaseViewModel
{
    public MyFileListPopup Instance { get; set; }

    public ObservableCollection<FileData> FileNames { get; } = new();

    //public ObservableCollection<Coordinate> Coordinates { get; set; } = new();

    public List<FileData> files = new List<FileData>();

    private readonly SqliteConnectionFactory _connectionFactory;

    [ObservableProperty]
    private string deleteFile;

    public MyFileListPopupViewModel(List<FileData> filed, SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;

        FileNames = new ObservableCollection<FileData>();

        files = new List<FileData>();
        files = filed;
        LoadFileCommand.Execute(null);

    }

    [RelayCommand]
    public void OkButton()
    {
        // Perform your logic here (e.g., saving the name)

        /*FileData fileData = new FileData
        {
            fileName = FileName,
            coordSystem = SelectedCoordSys,
            units = SelectedUnits
        };*/
        // Close the popup
        Instance?.Close();
    }


    [RelayCommand]
    async Task Delete(FileData file)
    {

        if (IsBusy)
            return;
        ISQLiteAsyncConnection database = _connectionFactory.CreateConnection();
        try
        {
            IsBusy = true;
            if (file is null)
                return;
            DeleteFile = file.fileName;
            var result = await AppShell.Current.DisplayAlert("Alert", "Are you sure you want to delete the selected file(s)?", "Yes", "No");
            if (result)
            {
                List<CoordinateDTO> coordinateDTOs = await database.Table<CoordinateDTO>().ToListAsync();

                var tempCoordinates = new List<Coordinate>();

                //await AppShell.Current.DisplayAlert("Error", $"{file1name}", "OK");
                //show();
                foreach (CoordinateDTO dto in coordinateDTOs)
                {

                    var coord = new Coordinate
                    {
                        //FirstName = "Nkosilamandla", // Setting properties after creation

                        //name = dto.name
                        id = dto.id,
                        name = dto.name,
                        y = dto.y,
                        x = dto.x,
                        z = dto.z,
                        coordSystem = dto.coordSystem,
                        units = dto.units,
                        fileName = dto.fileName

                    };
                    if (dto.fileName == DeleteFile)
                    {
                        await database.DeleteAsync(dto);
                    }
                }
                FileNames.Remove(new FileData { fileName = file.fileName, coordSystem = file.coordSystem, units = file.units });
            }
        }
        catch
        {

        }
        finally
        {

            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task LoadFile()
    {
        var tempFileNames = new List<FileData>();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            FileNames.Clear();
            foreach (var co in files)
            {
                co.IsChecked = false;
                FileNames.Add(co);
            }


        });
    }
    [ObservableProperty]
    private string file1Name;
    [ObservableProperty]
    private string file2Name;

    [RelayCommand]
    private async Task OpenFiles()
    {
        try
        {
            if (string.IsNullOrEmpty(File1Name) || string.IsNullOrEmpty(File2Name))
            {
                await AppShell.Current.DisplayAlert("Info", "Select 2 files!", "OK");
                return;
                //await AppShell.Current.GoToAsync($"{nameof(Surveyors_CalculatorPage2)}?search={name1.name}");

            }
            else
            {

                await AppShell.Current.GoToAsync($"{nameof(CalculationsPage)}?file1Key={File1Name}&file2Key={File2Name}");
            }
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            Instance?.Close();
        }
    }


    [RelayCommand]
    private void ToggleFile(FileData checkedFile)
    {
        checkedFile.IsChecked = !checkedFile.IsChecked;

        //var match = To.FirstOrDefault(s => s.name == checkedCoordinate.name);

        if (File1Name is null)
        {
            File1Name = checkedFile.fileName;
        }
        else
        {
            File2Name = checkedFile.fileName;
        }
    }


}

//List<CoordinateDTO> coordinateDTOs = await database.Table<CoordinateDTO>().ToListAsync();

//await AppShell.Current.DisplayAlert("Error", $"{file1name}", "OK");
//show();
/*foreach (CoordinateDTO dto in coordinateDTOs)
{
    var coord = new Coordinate
    {
        //FirstName = "Nkosilamandla", // Setting properties after creation

        //name = dto.name
        id = dto.id,
        name = dto.name,
        y = dto.y,
        x = dto.x,
        z = dto.z,
        coordSystem = dto.coordSystem,
        units = dto.units,
        fileName = dto.fileName

    };
    tempCoordinates.Add(coord);
    //From.Add(coord);
    // if (file2name == "")
}*/

/*if (fileNames.Count == 0)
{
    fileNames.Clear();
    //await AppShell.Current.DisplayAlert("Error", $" {File1[1]}", "OK");
    var fileNamesQueried = tempCoordinates.DistinctBy(s => s.fileName);
    foreach (var fileNamesQuery in fileNamesQueried)
    {
        fileNames.Add(fileNamesQuery.fileName);
        //await AppShell.Current.DisplayAlert("Error", $" {fileNamesQuery}", "OK");
    }
}*/