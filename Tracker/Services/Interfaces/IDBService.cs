using Tracker.Models;

namespace Tracker.Services.Interfaces;

public interface IDBService
{
    Task<int> Save(CustomLocation customLocation);
    Task<List<CustomLocation>> GetAll();
    Task<int> DeleteAll();
}