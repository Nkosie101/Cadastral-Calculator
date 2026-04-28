using System;
using System.IO;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Surveyors_Calculator.View
{
    public partial class About : ContentPage
    {
        public About(AboutViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }



    }
}