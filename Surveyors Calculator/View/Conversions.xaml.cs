using System;
using System.IO;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
//using Surveyors_Calculator.ViewModel;

namespace Surveyors_Calculator.View
{
    public partial class Conversions : ContentPage
    {
        public Conversions(ConversionsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}

