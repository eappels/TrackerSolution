using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Maps;
using Tracker.Messages;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace Tracker.ViewModels;

public partial class MainView : ContentPage, IDisposable
{

    private Distance mapDistance;

    public MainView()
	{
		InitializeComponent();

        mapDistance = Distance.FromKilometers(0.2);
        MyMap.MapElements.Add(((MainViewModel)BindingContext).Track);
        MyMap.PropertyChanging += (s, e) =>
        {
            if (e.PropertyName == nameof(Map.VisibleRegion))
            {
                if (MyMap.VisibleRegion == null)
                    return;
                mapDistance = MyMap.VisibleRegion.Radius;
            }
        };

        WeakReferenceMessenger.Default.Register<LocationUpdatedMessage>(this, (r, m) =>
        {
            var location = new Location(m.Value.Latitude, m.Value.Longitude);
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, mapDistance);
            MyMap.MoveToRegion(mapSpan);
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((MainViewModel)BindingContext).CheckPermissions();
    }

    public void Dispose()
    {
        if (MyMap != null)        
            MyMap.PropertyChanging -= (s, e) => { };        
    }
}