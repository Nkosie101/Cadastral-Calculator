using System;
using System.IO;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
//using Surveyors_Calculator.View;

namespace Surveyors_Calculator.View;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
