using System;
using System.Linq;
using System.Collections.Generic;
//using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;

namespace Surveyors_Calculator.Model;

public partial class Coordinate : ObservableObject
{
    public int id { get; set; }
    public string name { get; set; }
    public double x { get; set; }
    public double y { get; set; }
    public double z { get; set; }
    public string coordSystem { get; set; }
    public string units { get; set; }
    public string fileName { get; set; }

    [ObservableProperty]
    private bool isChecked;



}
