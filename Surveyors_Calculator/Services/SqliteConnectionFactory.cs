using System;
using System.Linq;
using System.Collections.Generic;
//using SQLite;
//using System.Threading.Tasks;
//using SQLite;
using System.IO;
using Microsoft.Maui.Storage;
//using Java.Nio.FileNio;


namespace Surveyors_Calculator;

public class SqliteConnectionFactory
{
    public SQLiteAsyncConnection CreateConnection()
    {
        return new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, "CoordinatesDatabase.db3"), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache, true);

    }
}
