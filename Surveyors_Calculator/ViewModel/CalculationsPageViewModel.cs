using System;
using System.Linq;
using System.Collections.Generic;
/*using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;*/
using Microsoft.Maui.Controls;
/*using Surveyors_Calculator.Model;
using SQLite;
using CSharpShellCore;
using Microsoft.Maui.ApplicationModel;*/
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Globalization;
using System.IO;
using Microsoft.Maui.Storage;

namespace Surveyors_Calculator.ViewModel;


[QueryProperty(nameof(File2name), "file2Key")]
[QueryProperty(nameof(File1name), "file1Key")]
public partial class CalculationsPageViewModel : BaseViewModel
{

    //[ObservableProperty]
    //string searchTerm;



    // Triggered when the search term arrives
    /*async partial void OnFile1nameChanged(string value)
    {
        //AppShell.Current.DisplayAlert("Error", $"{value}", "OK");
        file1name = value;
        LoadFileCommand.Execute(null);
        //SelectedFile1 = value;

    }*/

    //public string fileName1 { gawait AppShell.Current.DisplayAlert("Error", $"Check your entries! {From[1].name}", "OK");et; set; }
    [ObservableProperty]
    private string file1name = string.Empty;
    [ObservableProperty]
    private string file2name = string.Empty;
    [ObservableProperty]
    private string selectedFile1 = string.Empty;
    [ObservableProperty]
    private string selectedFile2 = string.Empty;

    [ObservableProperty]
    private string selectedCoordSys;
    [ObservableProperty]
    private string selectedUnits;
    [ObservableProperty]
    private string file1Name;
    public string mainpath = "CoordinateDatabase.csv";
    [ObservableProperty]
    private string exportPath;

    [ObservableProperty]
    private bool file1IsFrom = true;


    public ObservableCollection<Coordinate> Coordinates { get; set; } = new();
    public ObservableCollection<Coordinate> From { get; } = new();
    public ObservableCollection<Coordinate> To { get; } = new();
    //4public ObservableCollection<Coordinate> Nothing { get; } = new();
    public ObservableCollection<string> fileNames { get; } = new();




    private readonly SqliteConnectionFactory _connectionFactory;

    //Coordinate coordinate;
    Formulae formulae = new Formulae();

    //[QueryProperty(nameof(Formulae), "Formulae")]
    public CalculationsPageViewModel(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        Coordinates = new ObservableCollection<Coordinate>();
        From = new ObservableCollection<Coordinate>();
        To = new ObservableCollection<Coordinate>();
        //Nothing = new ObservableCollection<Coordinate>();
        fileNames = new ObservableCollection<string>();
        LoadFileCommand.Execute(null);

    }



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






    [RelayCommand]
    async Task Refresh()
    {
        IsRefreshing = true;
        try
        {
            //AppShell.Current.DisplayAlert("Error", $"File 1: {SelectedFile1} File 2: {SelectedFile2}", "OK");
            loadFileCommand.Execute(null);
        }
        catch (Exception e)
        {
            await AppShell.Current.DisplayAlert("Error", $"Check your entries! {e.Message}", "OK");
        }
        finally
        {
            IsRefreshing = false;
        }
    }
    /*

    [RelayCommand]
    async Task Refresh()
    {
        LoadFileCommand.Execute(null);
        IsRefreshing = false;

    }*/

    partial void OnSelectedFile1Changed(string value)
    {
        RefreshCommand.Execute(null);
    }
    partial void OnSelectedFile2Changed(string value)
    {
        RefreshCommand.Execute(null);
    }

    [RelayCommand]
    async Task Get3DTransformedCoordinates()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            //AppShell.Current.DisplayAlert("Error", $"E1 {e1Entry}, N1 {n1Entry}, Z1 {z1Entry}, E2 {e2Entry}, N2 {n2Entry}, Z2 {z2Entry}, E3 {e3Entry}, N3 {n3Entry}, Z3 {z3Entry}, E4 {e4Entry}, N4 {n4Entry}, Z4 {z4Entry}, E5 {e5Entry}, N5 {n5Entry}, Z5 {z5Entry}, E6 {e6Entry}, N6 {n6Entry}, Z6 {z6Entry}", "OK");
            formulae.createInitialMatrices(E1Entry, N1Entry, Z1Entry, E2Entry, N2Entry, Z2Entry, E3Entry, N3Entry, Z3Entry, E4Entry, N4Entry, Z4Entry, E5Entry, N5Entry, Z5Entry, E6Entry, N6Entry, Z6Entry);
            var rotation = formulae.SVD();
            formulae.S3D();
            formulae.Translation();

            for (int i = 0; i < From.Count; i++)
            {

                E7Entry = Convert.ToString(From[i].y);
                N7Entry = Convert.ToString(From[i].x);
                Z7Entry = Convert.ToString(From[i].z);

                var match = To.FirstOrDefault(s => s.name == From[i].name);

                if (match == null)
                {
                    var result = formulae.Transform3D(E7Entry, N7Entry, Z7Entry);
                    //AppShell.Current.DisplayAlert("Error", $"{e7Entry}", "OK");
                    Coordinate toCoord = new Coordinate
                    {
                        name = Convert.ToString(From[i].name),
                        x = Convert.ToDouble(result.Easting),
                        y = Convert.ToDouble(result.Northing),
                        z = Convert.ToDouble(result.Elevation),
                        IsChecked = false
                    };
                    To.Add(toCoord);
                }
            }
            await saveToHistory();
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
    async Task GetTransformedCoordinates()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var scaleFactor = formulae.GetScaleFactor(E1Entry, N1Entry, E2Entry, N2Entry, E3Entry, N3Entry, E4Entry, N4Entry);

            var rotation = formulae.GetRotation(E1Entry, N1Entry, E2Entry, N2Entry, E3Entry, N3Entry, E4Entry, N4Entry);

            formulae.GetTranslation(scaleFactor, rotation, E1Entry, N1Entry, E3Entry, N3Entry);

            for (int i = 0; i < From.Count; i++)
            {

                E5Entry = Convert.ToString(From[i].y);
                N5Entry = Convert.ToString(From[i].x);

                var match = To.FirstOrDefault(s => s.name == From[i].name);

                if (match == null)
                {
                    var result = formulae.Transform(E5Entry, N5Entry);
                    Coordinate toCoord = new Coordinate
                    {
                        name = Convert.ToString(From[i].name),
                        y = Convert.ToDouble(result.Easting),
                        x = Convert.ToDouble(result.Northing),
                        z = 0,
                        IsChecked = false
                    };
                    To.Add(toCoord);
                }
            }


        }
        catch (Exception e)
        {
            await AppShell.Current.DisplayAlert("Error", $"Check your entries! {e.Message}", "OK");
        }
        finally
        {
            await saveToHistory();
            IsBusy = false;

        }
    }

    int count = 0;
    [RelayCommand]
    private async Task LoadFile()
    {
        var tempCoordinates = new List<Coordinate>();
        var tempFrom = new List<Coordinate>();
        var tempTo = new List<Coordinate>();

        ISQLiteAsyncConnection database = _connectionFactory.CreateConnection();

        List<CoordinateDTO> coordinateDTOs = await database.Table<CoordinateDTO>().ToListAsync();

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
        count++;
        //AppShell.Current.DisplayAlert("3!", $"{count}", "OK");
        if (fileNames.Count == 0)
        {
            fileNames.Clear();
            //await AppShell.Current.DisplayAlert("Error", $" {File1[1]}", "OK");
            var fileNamesQueried = tempCoordinates.DistinctBy(s => s.fileName);
            foreach (var fileNamesQuery in fileNamesQueried)
            {
                fileNames.Add(fileNamesQuery.fileName);
                //await AppShell.Current.DisplayAlert("Error", $" {fileNamesQuery}", "OK");
            }
        }

        if (File1name == File2name)
        {
            await AppShell.Current.DisplayAlert("Error", "File names must be differrent!", "OK");
        }
        else
        {
            foreach (Coordinate coordinate in Coordinates)
            {
                if (!string.IsNullOrWhiteSpace(SelectedFile2))
                {
                    //AppShell.Current.DisplayAlert("Error", $" {SelectedFile2}", "OK");
                    File2name = SelectedFile2;
                }
                if (!string.IsNullOrWhiteSpace(SelectedFile1))
                {
                    File1name = SelectedFile1;
                    //AppShell.Current.DisplayAlert("Error", $"File 1: {SelectedFile1} File 2: {SelectedFile2}", "OK");
                }
                if (File1IsFrom == true)
                {
                    //From.Add(coord);

                    if (coordinate.fileName == File1name /*"Surveyors_Calculator1"*/)
                    {
                        //File1.Add(coord);
                        tempFrom.Add(coordinate);
                    }
                    else if (coordinate.fileName == File2name /*"Surveyors_Calculator2"*/)
                    {
                        //File2.Add(coord);
                        tempTo.Add(coordinate);
                    }
                }
                else
                {
                    if (coordinate.fileName == File1name)
                    {
                        //File1.Add(coord);
                        tempTo.Add(coordinate);
                    }
                    else if (coordinate.fileName == File2name)
                    {
                        //File2.Add(coord);
                        tempFrom.Add(coordinate);
                    }
                }
            }
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Coordinates.Clear();
            foreach (var co in tempCoordinates)
            {
                Coordinates.Add(co);
            }

            From.Clear();
            foreach (var co in tempFrom)
            {
                From.Add(co);
            }

            To.Clear();
            foreach (var co in tempTo)
            {
                To.Add(co);
            }


        });
    }

    [RelayCommand]
    private void TogglePoint(Coordinate checkedCoordinate)
    {
        checkedCoordinate.IsChecked = !checkedCoordinate.IsChecked;

        var match = To.FirstOrDefault(s => s.name == checkedCoordinate.name);

        if (match != null)
        {
            match.IsChecked = checkedCoordinate.IsChecked;

            if (!match.IsChecked)
            {
                if (Convert.ToDouble(E1Entry) == match.y && Convert.ToDouble(N1Entry) == match.x && Convert.ToDouble(Z1Entry) == match.z)
                {
                    E1Entry = string.Empty;
                    N1Entry = string.Empty;
                    Z1Entry = string.Empty;
                    E4Entry = string.Empty;
                    N4Entry = string.Empty;
                    Z4Entry = string.Empty;
                }
                else if (Convert.ToDouble(E2Entry) == match.y && Convert.ToDouble(N2Entry) == match.x && Convert.ToDouble(Z2Entry) == match.z)
                {
                    E2Entry = string.Empty;
                    N2Entry = string.Empty;
                    Z2Entry = string.Empty;
                    E5Entry = string.Empty;
                    N5Entry = string.Empty;
                    Z5Entry = string.Empty;
                }
                else //if (Convert.ToDouble(e2Entry) == match.y && Convert.ToDouble(n2Entry) == match.x && Convert.ToDouble(z2Entry) == match.z)
                {
                    E3Entry = string.Empty;
                    N3Entry = string.Empty;
                    Z3Entry = string.Empty;
                    E6Entry = string.Empty;
                    N6Entry = string.Empty;
                    Z6Entry = string.Empty;
                }
                //Parameters.Remove(checkedCoordinate);
                //Parameters.Remove(match);
            }
            else
            {
                if (E1Entry == "")
                {
                    E1Entry = Convert.ToString(match.y);
                    N1Entry = Convert.ToString(match.x);
                    Z1Entry = Convert.ToString(match.z);
                    E4Entry = Convert.ToString(checkedCoordinate.y);
                    N4Entry = Convert.ToString(checkedCoordinate.x);
                    Z4Entry = Convert.ToString(checkedCoordinate.z);
                }
                else if (E2Entry == "")
                {
                    E2Entry = Convert.ToString(match.y);
                    N2Entry = Convert.ToString(match.x);
                    Z2Entry = Convert.ToString(match.z);
                    E5Entry = Convert.ToString(checkedCoordinate.y);
                    N5Entry = Convert.ToString(checkedCoordinate.x);
                    Z5Entry = Convert.ToString(checkedCoordinate.z);
                }
                else if (E3Entry == "")
                {
                    E3Entry = Convert.ToString(match.y);
                    N3Entry = Convert.ToString(match.x);
                    Z3Entry = Convert.ToString(match.z);
                    E6Entry = Convert.ToString(checkedCoordinate.y);
                    N6Entry = Convert.ToString(checkedCoordinate.x);
                    Z6Entry = Convert.ToString(checkedCoordinate.z);
                }

                //Parameter.Add(checkedCoordinate);
                //Parameter.Add(match);
            }
        }
    }

    [RelayCommand]
    private async Task Swap()
    {
        File1IsFrom = !File1IsFrom;
        loadFileCommand.Execute(null);

    }

    async Task saveToHistory()
    {

        ISQLiteAsyncConnection database = _connectionFactory.CreateConnection();

        History history = new History
        {
            file1name = File1name,
            file2name = File2name,
            timeCalculated = DateTime.Now,
            numberOfPoints = To.Count + From.Count
        };
        await database.InsertAsync(history);
    }



    [RelayCommand]
    async Task ExportFile()
    {
        if (IsBusy)
            return;
        try
        {
            await SaveCommand.ExecuteAsync(null);
            List<CoordinateCSV> Output = new List<CoordinateCSV>();
            foreach (Coordinate coord in To)
            {
                CoordinateCSV outputcsv = new CoordinateCSV()
                {
                    id = coord.id,
                    name = coord.name,
                    y = coord.y,
                    x = coord.x,
                    z = coord.z,
                    /*coordSystem = coord.coordSystem,
                    units = coord.units,
                    fileName = coord.fileName*/
                };
                Output.Add(outputcsv);
            }
            using (var streamWriter = new StreamWriter($"{ExportPath}"))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false
                };
                using (var csvWriter = new CsvWriter(streamWriter, config))
                {
                    csvWriter.WriteRecords<CoordinateCSV>(Output);
                    //coordinateDTOs = csvReader.GetRecords<CoordinateCSV>().ToList();
                }
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

                ExportPath = File1Name + " " + SelectedCoordSys + "(" + SelectedUnits + ").csv";
                //await AppShell.Current.DisplayAlert("Error", $"{filedata.fileName}", "OK");
            }

        }
        /*await RefreshCommand.ExecuteAsync(null);
        await Task.Delay(100);
        if (!string.IsNullOrEmpty(PickedFile))
        {
            await ImportFileCommand.ExecuteAsync(null);
        }
        RefreshCommand.Execute(null);*/

    }

    /*public double lengthOfLine { get; set; }

    public double GetLengthOfLine(string E1, string N1, string E2, string N2)
    {

        lengthOfLine = Math.Sqrt(Math.Pow((Convert.ToDouble(E1) - Convert.ToDouble(E2)), 2)
                                       + Math.Pow((Convert.ToDouble(N1) - Convert.ToDouble(N2)), 2));

        return lengthOfLine;
    }

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

    //public ObservableCollection<Formulae> Formula { get; } 


    [RelayCommand]
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
            Debug.WriteLine(e);
            await AppShell.Current.DisplayAlert("Error", $"Check your entries! {e.Message}", "OK");
        }
        finally
        {

            IsBusy = false;

        }
        return;

    }*/


    //public ObservableCollection<Formulae> Formula { get; } 


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
            Debug.WriteLine(e);
            await AppShell.Current.DisplayAlert("Error", $"Check your entries! {e.Message}", "OK");
        }
        finally
        {

            IsBusy = false;

        }
        return;

    }*/

}