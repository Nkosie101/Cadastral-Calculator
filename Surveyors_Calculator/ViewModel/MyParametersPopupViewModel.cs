using System;
using System.Linq;
using System.Collections.Generic;
using Surveyors_Calculator.View;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Surveyors_Calculator.ViewModel;

//[QueryProperty(nameof(misclosure), "listKey")]
//[QueryProperty(nameof(Parameters), "matrixKey")]
public partial class MyParametersPopupViewModel : BaseViewModel
{

    //public ObservableCollection<string> CoordinateSystems { get; } = new();
    public ObservableCollection<Coordinate> TransformedControls { get; } = new();
    //Matrix<double> ParameterVector = Matrix<double>.Build.Dense(7, 1);


    public MyParametersPopupViewModel(Matrix<double> parameterVector, List<Coordinate> Controls)
    {
        ShowParameters(parameterVector, Controls);
    }


    [ObservableProperty]
    private string omega;
    [ObservableProperty]
    private string phi;
    [ObservableProperty]
    private string kappa;
    [ObservableProperty]
    private string scale;
    [ObservableProperty]
    private string translationX;
    [ObservableProperty]
    private string translationY;
    [ObservableProperty]
    private string translationZ;

    public void ShowParameters(Matrix<double> ParameterVector, List<Coordinate> Controls)
    {
        foreach (Coordinate deltaControl in Controls)
        {
            Coordinate dControl = new Coordinate
            {
                name = deltaControl.name,
                y = Convert.ToDouble($"{deltaControl.y:F3}"),
                x = Convert.ToDouble($"{deltaControl.x:F3}"),
                z = Convert.ToDouble($"{deltaControl.z:F3}"),

            };
            TransformedControls.Add(dControl);
        }

        Scale = $"{ParameterVector[0, 0]:F6}";
        Omega = $"{ParameterVector[1, 0]:F6}";
        Phi = $"{ParameterVector[2, 0]:F6}";
        Kappa = $"{ParameterVector[3, 0]:F6}";
        TranslationX = $"{ParameterVector[4, 0]:F3}";
        TranslationY = $"{ParameterVector[5, 0]:F3}";
        TranslationZ = $"{ParameterVector[6, 0]:F3}";
    }

    // Reference to the View
    public MyParametersPopup Instance { get; set; }

    [RelayCommand]
    public void OkButton()
    {
        Instance?.Close();
    }


}
