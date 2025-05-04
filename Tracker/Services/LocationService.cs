using Tracker.Models;
using Tracker.Services.Interfaces;

namespace Tracker.Services;

public partial class LocationService : ILocationService
{
    public Action<CustomLocation> OnLocationUpdate { get; set; }

    public void StartTracking(double distanceFilter)
    {
        StartTrackingInternal(distanceFilter);
    }

    public void StopTracking()
    {
        StopTracking();
    }

    partial void StartTrackingInternal(double distanceFilter);
    partial void StopTrackingInternal();
}