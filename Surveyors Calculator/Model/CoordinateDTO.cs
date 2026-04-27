using System;
using System.Linq;
using System.Collections.Generic;
//using SQLite;

namespace Surveyors_Calculator;

public class CoordinateDTO
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public string name { get; set; }
    public double x { get; set; }
    public double y { get; set; }
    public double z { get; set; }
    public string coordSystem { get; set; }
    public string units { get; set; }
    public string fileName { get; set; }

}
