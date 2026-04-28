using System;
using System.IO;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
//using CommunityToolkit.Mvvm.ComponentModel;
using Surveyors_Calculator.View;

namespace Surveyors_Calculator.View
{
    public partial class CalculationsMenuPage : ContentPage
    {
        public CalculationsMenuPage(CalculationsMenuPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}