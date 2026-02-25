using System;
using System.IO;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Surveyors_Calculator.Popups
{
    public partial class MyAlertPopup : Popup
    {
        public MyAlertPopup(MyAlertPopupViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            //viewModel.Instance = this;
        }
    }
}