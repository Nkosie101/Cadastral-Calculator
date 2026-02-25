using System;
using System.Linq;
using System.Collections.Generic;
using Surveyors_Calculator.View;

namespace Surveyors_Calculator.ViewModel;

public partial class RecentViewModel : BaseViewModel
{
    public ObservableCollection<History> RecentCalcs { get; set; } = new();

    public RecentViewModel(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        RecentCalcs = new ObservableCollection<History>();
        LoadRecent();
    }

    private readonly SqliteConnectionFactory _connectionFactory;

    [RelayCommand]
    async Task Refresh()
    {
        IsRefreshing = true;
        try
        {
            //AppShell.Current.DisplayAlert("Error", $"File 1: {SelectedFile1} File 2: {SelectedFile2}", "OK");
            await LoadRecent();
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    // var match = recentTemp.Where(s => );
    async Task LoadRecent()
    {
        try
        {
            IsBusy = true;
            ISQLiteAsyncConnection database = _connectionFactory.CreateConnection();
            List<History> recentTemp = await database.Table<History>().ToListAsync();
            var sorted = recentTemp.OrderByDescending(s => s.timeCalculated).DistinctBy(n => new { n.file1name, n.file2name }).ToList();
            RecentCalcs.Clear();
            foreach (History recentItem in sorted)
            {
                if (true)
                {
                    RecentCalcs.Add(recentItem);
                }
            }

        }
        catch (Exception e)
        {
            await AppShell.Current.DisplayAlert("Info", e.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [ObservableProperty]
    private string file1Name;
    [ObservableProperty]
    private string file2Name;

    [RelayCommand]
    private async Task OpenRecentFiles()
    {
        try
        {
            await AppShell.Current.GoToAsync($"{nameof(CalculationsPage)}?file1Key={File1Name}&file2Key={File2Name}");
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {

        }
    }


    [RelayCommand]
    private void ToggleRecent(History clickedFile)
    {
        try
        {
            File1Name = clickedFile.file1name;
            File2Name = clickedFile.file2name;
            if (!string.IsNullOrEmpty(File1Name))
            {
                OpenRecentFilesCommand.Execute(null);
            }
        }
        catch (Exception ex)
        {
            AppShell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {

        }

    }
}
