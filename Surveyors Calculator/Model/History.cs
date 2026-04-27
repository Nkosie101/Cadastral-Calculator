using System;
using System.Linq;
using System.Collections.Generic;


namespace Surveyors_Calculator.Model;

public partial class History : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public string file1name { get; set; }
    public string file2name { get; set; }
    public DateTime timeCalculated { get; set; }
    public int numberOfPoints { get; set; }

}
