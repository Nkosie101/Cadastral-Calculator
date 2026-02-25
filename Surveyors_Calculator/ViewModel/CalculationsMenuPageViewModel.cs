using System;
using System.Linq;
using System.Collections.Generic;
//using CommunityToolkit.Mvvm.ComponentModel;
//using System.Threading.Tasks;
using Surveyors_Calculator.View;
//using Surveyors_Calculator.Model;


namespace Surveyors_Calculator.ViewModel;


public partial class CalculationsMenuPageViewModel : BaseViewModel
{
    public ObservableCollection<Formulae> Formulas { get; set; } = new();


    public CalculationsMenuPageViewModel()
    {
        //formulae = new Formulae { name = "Join", image = "join_image.avif" };
        //Formulas.Add(formulae);
        Formulas = new ObservableCollection<Formulae>();
        AddFormulae();
    }




    [ObservableProperty]
    private string nameOfFormula = string.Empty;

    [RelayCommand]
    async Task GoToFormula(Formulae formulae)
    {
        if (formulae is null)
            return;

        await AppShell.Current.GoToAsync(nameof(CalculationsPage), true,
            new Dictionary<string, object>
            {
                {"Formulae", formulae}
            });
    }

    public void AddFormulae()
    {
        Formulae formulae1 = new Formulae { name = "Join", image = "join_image.avif" };
        Formulae formulae2 = new Formulae { name = "Polar", image = "polar_image.webp" };
        Formulae formulae3 = new Formulae { name = "Intersection", image = "join_image.avif" };
        Formulae formulae4 = new Formulae { name = "Resection", image = "join_image.avif" };
        Formulas.Add(formulae1);
        Formulas.Add(formulae2);
        Formulas.Add(formulae3);
        Formulas.Add(formulae4);
    }
}




