using System;
using System.IO;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
//using CommunityToolkit.Maui.Views;

namespace Surveyors_Calculator.Popups
{
    public partial class MyParametersPopup : Popup
    {
        public MyParametersPopup(MyParametersPopupViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            viewModel.Instance = this;
        }


    }
}