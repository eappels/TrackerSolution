using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Maps;
using Tracker.Messages;
using Tracker.Models;
using Tracker.Services.Interfaces;
using Tracker.Views;

namespace Tracker.ViewModels;

public partial class MainViewModel : ObservableObject
{

    private readonly ILocationService locationService;

    public MainViewModel(ILocationService locationService)
    {
        Track = new Polyline
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 5
        };

        this.locationService = locationService;
    }

    public async Task CheckPermissions()
    {
        var status = PermissionStatus.Unknown;
        status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status == PermissionStatus.Granted)
        {
            this.locationService.OnLocationUpdate += OnLocationUpdate;
            this.locationService.StartTracking(0);
            return;
        }
        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
            App.Current.MainPage = new NavigationPage(new PermissionsDeniedView());
                
        this.locationService.OnLocationUpdate += OnLocationUpdate;
        this.locationService.StartTracking(0);        
    }

    private void OnLocationUpdate(CustomLocation customLocation)
    {
        WeakReferenceMessenger.Default.Send(new LocationUpdatedMessage(customLocation));
        Track.Add(new Location(customLocation.Latitude, customLocation.Longitude));
    }

    [ObservableProperty]
    private Polyline track;
}