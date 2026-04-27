using System;
using System.IO;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Surveyors_Calculator.View
{
    public partial class Settings : ContentPage
    {
        public Settings(SettingsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

        }
    }
}