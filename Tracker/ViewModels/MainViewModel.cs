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
    private readonly IDBService dbService;

    public MainViewModel(ILocationService locationService, IDBService dbService)
    {
        DistanceInMeters = 500;
        Track = new Polyline
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 5
        };
        this.locationService = locationService;
        this.dbService = dbService;
    }

    public async Task CheckPermissions()
    {
        var status = PermissionStatus.Unknown;
        status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status == PermissionStatus.Granted)
        {
            await OnPermissionsGranted();
            return;
        }
        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
            App.Current.MainPage.Navigation.PushAsync(new PermissionsDeniedView());
        await OnPermissionsGranted();
    }

    private async Task OnPermissionsGranted()
    {
        this.locationService.OnLocationUpdate += OnLocationUpdate;
        this.locationService.StartTracking(DistanceInMeters);
        var list = await dbService.GetAll();
        foreach (var item in list)
            Track.Add(new Location(item.Latitude, item.Longitude));        
    }

    private async void OnLocationUpdate(CustomLocation customLocation)
    {       
        Track.Add(new Location(customLocation.Latitude, customLocation.Longitude));
        WeakReferenceMessenger.Default.Send(new LocationUpdatedMessage(customLocation));        
        var returnData = 0;
        returnData = await dbService.Save(customLocation);
    }

    [ObservableProperty]
    private Polyline track;

    [ObservableProperty]
    private double distanceInMeters;

    //partial void OnDistanceInMetersChanged(double oldValue, double newValue)
    //{
    //    throw new NotImplementedException();
    //}
}