using System;
using System.IO;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Surveyors_Calculator.View
{
    public partial class QuickTransform : ContentPage
    {
        public QuickTransform(QuickTransformViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

        }
    }
}