using System;
using System.Linq;
using System.Collections.Generic;
/*using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;*/
using Surveyors_Calculator.View;
using Surveyors_Calculator.Services;
using Android.App;
//using SQLite;
using Microsoft.Maui.Graphics;
//using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core.Views;
using AndroidX.Core.App;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Globalization;
using System.IO;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;

namespace Surveyors_Calculator.ViewModel;

[QueryProperty(nameof(PickedFile), "file1Key")]
public partial class InputViewModel : BaseViewModel
{

    public ObservableCollection<Coordinate> Coordinates { get; } = new();
    public ObservableCollection<Coordinate> CoordinatesFromDB { get; } = new();
    public ObservableCollection<string> CoordinateSystems { get; } = new();
    public ObservableCollection<string> Units { get; } = new();


    private readonly SqliteConnectionFactory _connectionFactory;
    [ObservableProperty]
    private string pickedFile = string.Empty;

    public InputViewModel(SqliteConnectionFactory connectionFactory/*, IPopupService popupService*/)
    {

        _connectionFactory = connectionFactory;
        //_popupService = popupService;
        Coordinates = new ObservableCollection<Coordinate>();
        CoordinatesFromDB = new ObservableCollection<Coordinate>();

        Units.Add("metres");
        Units.Add("Inches");
        CoordinateSystems.Add("Lo 27°");
        CoordinateSystems.Add("Lo 29°");
        CoordinateSystems.Add("Lo 31°");


        SaveCommand.Execute(null);

    }


    [ObservableProperty]
    private string e1Entry = string.Empty;
    [ObservableProperty]
    private string n1Entry = string.Empty;
    [ObservableProperty]
    private string z1Entry = string.Empty;
    [ObservableProperty]
    private string name = string.Empty;
    [ObservableProperty]
    private string y = string.Empty;
    [ObservableProperty]
    private string x = string.Empty;
    [ObservableProperty]
    private string z = string.Empty;
    [ObservableProperty]
    private string selectedCoordSys;
    [ObservableProperty]
    private string selectedUnits;
    [ObservableProperty]
    private string file1Name;
    [ObservableProperty]
    private string topLabel;
    [ObservableProperty]
    private int updateID = -1;
    [ObservableProperty]
    private Color editing;

    void ClearInputs()
    {
        Y = string.Empty;
        X = string.Empty;
        Z = string.Empty;
        Name = string.Empty;

        UpdateID = -1;
        Editing = Colors.Transparent;
    }

    [RelayCommand]
    async Task Refresh()
    {
        IsRefreshing = true;
        try
        {
            //AppShell.Current.DisplayAlert("Error", $"File 1: {SelectedFile1} File 2: {SelectedFile2}", "OK");
            await LoadFile();
        }
        catch (Exception e)
        {
            await AppShell.Current.DisplayAlert("Error", $"! {e.Message}", "OK");
        }
        finally
        {
            ClearInputs();
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    async Task Delete()
    {
        if (IsBusy)
            return;
        ISQLiteAsyncConnection database = _connectionFactory.CreateConnection();
        try
        {
            IsBusy = true;
            var result = await AppShell.Current.DisplayAlert("Alert", "Are you sure you want to delete the selected file(s)?", "Yes", "No");
            if (result)
            {

                foreach (Coordinate coordinate in Coordinates)
                {
                    if (coordinate.IsChecked is true)
                    {
                        CoordinateDTO coordinatedto = new CoordinateDTO()
                        {
                            id = coordinate.id,
                            name = coordinate.name,
                            y = coordinate.y,
                            x = coordinate.x,
                            z = coordinate.z,
                            units = coordinate.units,
                            coordSystem = coordinate.coordSystem,
                            fileName = coordinate.fileName
                        };
                        //AppShell.Current.DisplayAlert("Error!", $"{coordinatedto.id}", "OK");
                        await database.DeleteAsync(coordinatedto.id);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            RefreshCommand.Execute(null);
            IsBusy = false;
        }
    }

    [RelayCommand]
    async Task DisplayCoordinate(Coordinate coordinate)
    {
        if (coordinate is null)
            return;
        Y = Convert.ToString(coordinate.y);
        X = Convert.ToString(coordinate.x);
        Z = Convert.ToString(coordinate.z);
        Name = coordinate.name;
        SelectedUnits = coordinate.units;
        SelectedCoordSys = coordinate.coordSystem;
        UpdateID = coordinate.id;

        Editing = Colors.LimeGreen;

    }

    [RelayCommand]
    async Task Update()
    {
        if (IsBusy)
            return;
        ISQLiteAsyncConnection database = _connectionFactory.CreateConnection();
        try
        {
            IsBusy = true;

            /*string sql = "UPDATE Coordinates SET x=?,y=?,z=? WHERE id=?" ;
            await database.ExecuteAsync(sql,Convert.ToDouble(X), Convert.ToDouble(Y), Convert.ToDouble(Z), UpdateID);*/
            //foreach (Coordinate coordinate in Coordinates)
            var coordinate = Coordinates.FirstOrDefault(p => p.id == UpdateID);


            CoordinateDTO coordinatedto = new CoordinateDTO()
            {
                name = coordinate.name,
                id = coordinate.id,
                y = Convert.ToDouble(Y),
                x = Convert.ToDouble(X),
                z = Convert.ToDouble(Z),
                coordSystem = SelectedCoordSys,
                units = SelectedUnits,
                fileName = File1Name
            };

            await database.UpdateAsync(coordinatedto);
            //AppShell.Current.DisplayAlert("Error!", $"{coordinatedto.id}", "OK");
            /*var match = Coordinates.Where(s => s.name == coordinatedto.name && s.id != UpdateID);
            if (match == null)
            {
                AppShell.Current.DisplayAlert("Error!", $"{coordinatedto.coordSystem}, {coordinatedto.units}", "OK");
                //await database.UpdateAsync(coordinatedto);
            }
            foreach(Coordinate nameCheck in Coordinates)
            {
                if(nameCheck.name == coordinatedto.name && nameCheck.id==UpdateID)
                {
                    AppShell.Current.DisplayAlert("Error!", $"{coordinatedto.coordSystem}, {coordinatedto.units}", "OK");
                }
            }*/



        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            RefreshCommand.Execute(null);
            IsBusy = false;
        }
    }

    [RelayCommand]
    async Task Done()
    {
        if (string.IsNullOrEmpty(File1Name))
        {
            await AppShell.Current.DisplayAlert("Error", "Save the file first!", "OK");
            return;
            //await AppShell.Current.GoToAsync($"{nameof(Surveyors_CalculatorPage2)}?search={name1.name}");

        }
        else
        {
            await AppShell.Current.GoToAsync($"{nameof(CalculationsPage)}?file1Key={File1Name}&file2Key=");
        }

        //

    }

    [RelayCommand]
    public async Task Save()
    {
        //if (string.IsNullOrEmpty(fileName)
        {

            /*var result = await AppShell.Current.DisplayPromptAsync("File Name", "File name must be unique to avoid merging with existing file", "OK", "Cancel");
            */
            var viewModel = new MySavePopupViewModel();
            var popup = new MySavePopup(viewModel);
            var result = await AppShell.Current.ShowPopupAsync(popup);

            if (result is FileData filedata)
            {


                File1Name = filedata.fileName;
                SelectedCoordSys = filedata.coordSystem;
                SelectedUnits = filedata.units;





                TopLabel = File1Name + " " + SelectedCoordSys + "(" + SelectedUnits + ")";
                //await AppShell.Current.DisplayAlert("Error", $"{filedata.fileName}", "OK");
            }

            //await _popupService.ShowAlertAsync("Hello", "This is a clean MVVM popup!", "OK");


        }
        await RefreshCommand.ExecuteAsync(null);
        await Task.Delay(100);
        if (!string.IsNullOrEmpty(PickedFile))
        {
            await ImportFileCommand.ExecuteAsync(null);
        }
        RefreshCommand.Execute(null);

    }



    [RelayCommand]
    async Task AddCoordinates()
    {

        //else (!string.IsNullOrEmpty(File1Name) && !string.IsNullOrEmpty(SelectedCoordSys) && !string.IsNullOrEmpty(SelectedUnits))

        if (IsBusy)
            return;

        try
        {

            IsBusy = true;
            if (string.IsNullOrEmpty(TopLabel))
            {
                await AppShell.Current.DisplayAlert("Info", "Save the file before entering coordinates", "OK");
            }
            else if (UpdateID != -1)
            {
                UpdateCommand.Execute(null);

            }
            else
            {
                ISQLiteAsyncConnection database = _connectionFactory.CreateConnection();

                CoordinateDTO coordinateDTO = new CoordinateDTO()
                {
                    name = Name,
                    y = Convert.ToDouble(Y),
                    x = Convert.ToDouble(X),
                    z = Convert.ToDouble(Z),
                    coordSystem = SelectedCoordSys,
                    units = SelectedUnits,
                    fileName = File1Name
                };



                List<CoordinateDTO> coordinateDTOs = await database.Table<CoordinateDTO>().ToListAsync();

                //await AppShell.Current.DisplayAlert("Error", $"{file1name}", "OK");
                //show();
                foreach (CoordinateDTO dto in coordinateDTOs)
                {
                    var coord = new Coordinate
                    {
                        id = dto.id,
                        name = dto.name,
                        y = dto.y,
                        x = dto.x,
                        z = dto.z,
                        coordSystem = dto.coordSystem,
                        units = dto.units,
                        fileName = dto.fileName
                    };
                    CoordinatesFromDB.Add(coord);
                }


                var match = CoordinatesFromDB.FirstOrDefault(s => s.name == coordinateDTO.name && s.fileName == coordinateDTO.fileName);

                if (match == null)
                {
                    await database.InsertAsync(coordinateDTO);

                    Coordinate trial5 = new Coordinate()
                    {
                        id = coordinateDTO.id,
                        name = coordinateDTO.name,
                        y = coordinateDTO.y,
                        x = coordinateDTO.x,
                        z = coordinateDTO.z,
                        coordSystem = coordinateDTO.coordSystem,
                        units = coordinateDTO.units,
                        fileName = coordinateDTO.fileName
                        //name = "empty"
                    };

                    Coordinates.Add(trial5);
                }

                else
                {
                    await AppShell.Current.DisplayAlert("Error!", "Coordinate already exists", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            Name = "";
            Y = "";
            X = "";
            Z = "";


            IsBusy = false;
        }


    }




    async Task LoadFile()
    {
        ISQLiteAsyncConnection database = _connectionFactory.CreateConnection();

        List<CoordinateDTO> coordinateDTOs = await database.Table<CoordinateDTO>().ToListAsync();

        var tempCoordinates = new List<Coordinate>();

        //await AppShell.Current.DisplayAlert("Error", $"{file1name}", "OK");
        //show();
        foreach (CoordinateDTO dto in coordinateDTOs)
        {
            if (dto.fileName == File1Name)
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
            }
            //From.Add(coord);
            // if (file2name == "")
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Coordinates.Clear();
            foreach (var co in tempCoordinates)
            {
                Coordinates.Add(co);
            }

        });
    }

    [RelayCommand]
    async Task ImportFile()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            bool modified = false;
            ISQLiteAsyncConnection database = _connectionFactory.CreateConnection();

            //List<CoordinateCSV> coordinateDTOs = new List<CoordinateCSV>();
            List<CoordinateCSV> importDTOs = new List<CoordinateCSV>();
            //List<CoordinateCSV> coordinateDTOs = new List<CoordinateCSV>();
            var importCoordinatesFromDB = new List<Coordinate>();


            await Task.Delay(100);
            var path = PickedFile;

            using (var streamReader = new StreamReader($"{PickedFile}"))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                    MissingFieldFound = null
                };
                using (var csvReader = new CsvReader(streamReader, config))
                {
                    importDTOs = csvReader.GetRecords<CoordinateCSV>().ToList();
                    foreach (var dto in importDTOs)
                    {
                        dto.coordSystem = SelectedCoordSys;
                        dto.units = SelectedUnits;
                        dto.fileName = File1Name;
                    }
                }
            }
            //AppShell.Current.DisplayAlert("Error", $"{PickedFile}", "OK");
            List<CoordinateDTO> coordinateDTOs = await database.Table<CoordinateDTO>().ToListAsync();

            //await AppShell.Current.DisplayAlert("Error", $"{file1name}", "OK");
            //show();
            foreach (CoordinateDTO dto in coordinateDTOs)
            {
                var coord = new Coordinate
                {
                    id = dto.id,
                    name = dto.name,
                    y = dto.y,
                    x = dto.x,
                    z = dto.z,
                    coordSystem = dto.coordSystem,
                    units = dto.units,
                    fileName = dto.fileName
                };
                importCoordinatesFromDB.Add(coord);
            }

            foreach (var importdto in importDTOs)
            {
                Coordinate trial5 = new Coordinate()
                {
                    id = importdto.id,
                    name = importdto.name,
                    y = importdto.y,
                    x = importdto.x,
                    z = importdto.z,
                    coordSystem = SelectedCoordSys,
                    units = SelectedUnits,
                    fileName = File1Name
                    //name = "empty"
                };
                CoordinateDTO dto = new CoordinateDTO()
                {
                    //id = importdto.id,
                    name = importdto.name,
                    y = importdto.y,
                    x = importdto.x,
                    z = importdto.z,
                    coordSystem = SelectedCoordSys,
                    units = SelectedUnits,
                    fileName = File1Name
                    //name = "empty"
                };
                var match = importCoordinatesFromDB.FirstOrDefault(s => s.name == importdto.name && s.fileName == importdto.fileName);
                if (match == null)
                {
                    //tempCoordinates.Add(trial5);
                    await database.InsertAsync(dto);
                }
                else
                {
                    modified = true;
                    while (match != null)
                    {
                        trial5.name = trial5.name + " (1)";

                        match = importCoordinatesFromDB.FirstOrDefault(s => s.name == importdto.name && s.fileName == importdto.fileName);
                    }

                    await database.InsertAsync(dto);
                }
                Coordinates.Add(trial5);
            }
            if (modified == true)
            {
                AppShell.Current.DisplayAlert("Info!", "Point names have been modified due to duplication", "OK");
            }



        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Error", $"Check your entries! {ex.Message}", "OK");
        }
        finally
        {

            IsBusy = false;
        }
    }



}
