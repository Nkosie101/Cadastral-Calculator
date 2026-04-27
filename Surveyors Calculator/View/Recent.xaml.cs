using System;
using System.IO;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Surveyors_Calculator.View
{
    public partial class Recent : ContentPage
    {
        public Recent(RecentViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}