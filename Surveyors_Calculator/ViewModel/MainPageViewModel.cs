using System;
using System.Linq;
using System.Collections.Generic;
//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
using Surveyors_Calculator.View;
//using Surveyors_Calculator.Model;
//using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace Surveyors_Calculator.ViewModel;

public partial class MainPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private string e1Entry = string.Empty;
    [ObservableProperty]
    private string n1Entry = string.Empty;
    [ObservableProperty]
    private string e2Entry = string.Empty;
    [ObservableProperty]
    private string n2Entry = string.Empty;
    [ObservableProperty]
    private string length = "0";
    [ObservableProperty]
    private string fileName = "Surveyors_Calculator1";
    [ObservableProperty]
    private string pickedFile = string.Empty;

    private readonly SqliteConnectionFactory _connectionFactory;

    public List<FileData> FileNames { get; } = new();

    //public ObservableCollection<Formulae> Formula { get; } = new();

    Formulae formulae = new Formulae();

    public MainPageViewModel(Formulae formulae, SqliteConnectionFactory connectionFactory)
    {
        SetTheme();
        Title = "Surveyors Calculator";
        this.formulae = formulae;
        _connectionFactory = connectionFactory;

        LoadFile();
    }

    async Task SetTheme()
    {
        bool isDark = Preferences.Default.Get("AppThemeDark", false);
        if (isDark)
        {
            Microsoft.Maui.Controls.Application.Current.UserAppTheme = AppTheme.Dark;
        }
        else
        {
            Microsoft.Maui.Controls.Application.Current.UserAppTheme = AppTheme.Light;
        }
    }

    [RelayCommand]
    async Task GoToCalculationsMenu()
    {
        await AppShell.Current.GoToAsync($"{nameof(CalculationsMenuPage)}");
        //await Shell.Current.GoToAsync($"{nameof(CalculationsMenuPage)}");
    }

    [RelayCommand]
    async Task GoToInput()
    {
        await AppShell.Current.GoToAsync($"{nameof(Input)}");

    }

    [RelayCommand]
    async Task GoToImport()
    {
        await ImportFile();
        //AppShell.Current.DisplayAlert("Error", $"{PickedFile}", "OK");
        await AppShell.Current.GoToAsync($"{nameof(Input)}?file1Key={PickedFile}");

    }

    [RelayCommand]
    async Task FileList()
    {
        var viewModel = new MyFileListPopupViewModel(FileNames, _connectionFactory);
        var popup = new MyFileListPopup(viewModel);
        var result = await AppShell.Current.ShowPopupAsync(popup);
    }

    public List<FileData> listOfFiles = new List<FileData>();

    async Task LoadFile()
    {
        ISQLiteAsyncConnection database = _connectionFactory.CreateConnection();

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
            tempCoordinates.Add(coord);

            //From.Add(coord);
            // if (file2name == "")
        }

        var listOfFiles = tempCoordinates.DistinctBy(f => f.fileName).ToList();


        foreach (Coordinate coordinate in listOfFiles)
        {
            var fileCoordSystem = tempCoordinates.Where(n => n.fileName == coordinate.fileName).Select(n => n.coordSystem).FirstOrDefault();
            var fileUnits = tempCoordinates.Where(n => n.fileName == coordinate.fileName).Select(n => n.units).FirstOrDefault();
            var points = tempCoordinates.Where(n => n.fileName == coordinate.fileName).Count();
            FileData file = new FileData
            {
                fileName = coordinate.fileName,
                coordSystem = fileCoordSystem,
                units = fileUnits,
                numberOfPoints = Convert.ToString(points)

            };
            FileNames.Add(file);
        }


        /*MainThread.BeginInvokeOnMainThread(() =>
        {
            
            Coordinates.Clear();
            foreach (var co in tempCoordinates)
            {
                Coordinates.Add(co);
            }

        });*/
    }

    async Task ImportFile()
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Pick a file",
            //FileTypes= FilePickerFileType

        });

        if (result == null)
            return;
        var stream = await result.OpenReadAsync();
        PickedFile = result.FullPath;

    }

    /*[RelayCommand]
    async Task GetLengthOfLine()
    {
        //length = "something";
        if (IsBusy)
            return;

        try
        {
            length = "something";
            IsBusy = true;
            var lengthOfLine = GetLengthOfLine(e1Entry, n1Entry, e2Entry, n2Entry);
            length = Convert.ToString(lengthOfLine);

        }
        catch (Exception e)
        {
            await AppShell.Current.DisplayAlert("Error", $"Check your entries! {e.Message}", "OK");
        }
        finally
        {

            IsBusy = false;

        }
        return;
    }*/


}
