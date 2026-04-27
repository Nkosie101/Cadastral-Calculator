using System;
using System.IO;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Surveyors_Calculator.View
{
    public partial class Input : ContentPage
    {
        public Input(InputViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}