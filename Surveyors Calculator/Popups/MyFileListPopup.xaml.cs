using System;
using System.IO;
using Microsoft.Maui;
using Microsoft.Maui.Controls;


namespace Surveyors_Calculator.Popups
{
    public partial class MyFileListPopup : Popup
    {
        public MyFileListPopup(MyFileListPopupViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            viewModel.Instance = this;
        }
    }
}