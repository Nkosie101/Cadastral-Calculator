using System;
using System.Linq;
using System.Collections.Generic;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace Surveyors_Calculator.Model;

public partial class CoordinateCSVMap : ClassMap<CoordinateCSV>
{


    public CoordinateCSVMap(int name, int y, int x, int z)
    {
        //MapColumns();
        Map(m => m.name).Index(name);
        Map(m => m.y).Index(y);
        Map(m => m.x).Index(x);
        Map(m => m.z).Index(z);
    }
}
