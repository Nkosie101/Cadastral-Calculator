using System;
using System.Linq;
using System.Collections.Generic;
//using LINQtoCSV;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

//using SQLite;

namespace Surveyors_Calculator.Model;


public class CoordinateCSV
{

    /*[Index(0)]
    public int id { get; set; }
    [Index(1)]
    public string name { get; set; }
    [Index(2)]
    public double x { get; set; }
    [Index(3)]
    public double y { get; set; }
    [Index(4)]
    public double z { get; set; }
    [Index(5)]
    public string coordSystem { get; set; }
    [Index(6)]
    public string units { get; set; }
    [Index(7)]
    */

    public string name { get; set; }

    public double x { get; set; }

    public double y { get; set; }

    public double z { get; set; }

    public string fileName { get; set; }







}
