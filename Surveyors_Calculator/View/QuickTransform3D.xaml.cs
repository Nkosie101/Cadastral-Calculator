using System;
using System.IO;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Surveyors_Calculator.View
{
    public partial class QuickTransform3D : ContentPage
    {
        public QuickTransform3D(QuickTransform3DViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

        }
    }
}