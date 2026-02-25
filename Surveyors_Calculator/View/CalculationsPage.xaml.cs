using System;
using System.IO;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
//using Surveyors_Calculator.ViewModel;

namespace Surveyors_Calculator.View
{
    public partial class CalculationsPage : ContentPage
    {
        public CalculationsPage(CalculationsPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}

