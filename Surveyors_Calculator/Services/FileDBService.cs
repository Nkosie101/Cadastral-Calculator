using System;
using System.Linq;
using System.Collections.Generic;
//using System.Threading.Tasks;
//using SQLite;
using System.IO;
using Microsoft.Maui.Storage;
//using Java.Nio.FileNio;


namespace Surveyors_Calculator.Services;

public class FileDBService
{
    SQLiteAsyncConnection db;
    async Task Init()
    {
        if (db != null)
            return;

        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "CoordinatesDatabase.db");

        db = new SQLiteAsyncConnection(databasePath);

        await db.CreateTableAsync<Coordinate>();
    }

    public async Task AddCoordinate(string name1)
    {
        await Init();
        var Coordinate1 = new Coordinate
        {
            name = name1
        };

        var id = await db.InsertAsync(Coordinate1);
    }

    public async Task RemoveCoordinate(int id)
    {
        await Init();

        await db.DeleteAsync<Coordinate>(id);
    }

    public async Task<IEnumerable<Coordinate>> GetCoordinate()
    {
        await Init();

        var Coordinate1 = await db.Table<Coordinate>().ToListAsync();
        return Coordinate1;
    }

}
