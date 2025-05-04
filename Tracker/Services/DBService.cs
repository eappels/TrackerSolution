using SQLite;
using Tracker.Helpers;
using Tracker.Models;
using Tracker.Services.Interfaces;

namespace Tracker.Services;

public class DBService : IDBService
{

    private SQLiteAsyncConnection database;

    private async Task Init()
    {
        if (database != null)
            return;

        database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        var result = await database.CreateTableAsync<CustomLocation>();
    }

    public async Task<int> Save(CustomLocation customLocation)
    {
        await Init();
        var result = await database.InsertAsync(customLocation);
        return result;
    }

    public async Task<List<CustomLocation>> GetAll()
    {
        await Init();
        return await database.Table<CustomLocation>().ToListAsync();
    }

    public async Task<int> DeleteAll()
    {
        await Init();
        return await database.DeleteAllAsync<CustomLocation>();
    }
}