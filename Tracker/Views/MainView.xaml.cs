using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Maps;
using System.Diagnostics;
using Tracker.Messages;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace Tracker.ViewModels;

public partial class MainView : ContentPage, IDisposable
{

    private Distance mapDistance;

    public MainView()
	{
		InitializeComponent();

        mapDistance = Distance.FromMeters(((MainViewModel)BindingContext).DistanceInMeters);

        WeakReferenceMessenger.Default.Register<LocationUpdatedMessage>(this, (r, m) =>
        {
            var location = new Location(m.Value.Latitude, m.Value.Longitude);
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, mapDistance);
            MyMap.MoveToRegion(mapSpan);
        });

        MyMap.PropertyChanging += (s, e) =>
        {
            if (e.PropertyName == nameof(Map.VisibleRegion))
            {
                if (MyMap.VisibleRegion == null)
                    return;
                mapDistance = MyMap.VisibleRegion.Radius;
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((MainViewModel)BindingContext).CheckPermissions();

        try
        {
            MyMap.MapElements.Add(((MainViewModel)BindingContext).Track);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex, $"Error When adding polyline to map: {ex.InnerException}");
            throw;
        }
    }

    public void Dispose()
    {
        if (MyMap != null)        
            MyMap.PropertyChanging -= (s, e) => { };        
    }
}