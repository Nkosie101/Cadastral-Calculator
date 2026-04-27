using System;
using System.Linq;
using System.Collections.Generic;

namespace Surveyors_Calculator.Model;

public partial class FileData : ObservableObject
{
    public string fileName { get; set; }
    public string coordSystem { get; set; }
    public string units { get; set; }
    public string numberOfPoints { get; set; }

    [ObservableProperty]
    private bool isChecked;
}
