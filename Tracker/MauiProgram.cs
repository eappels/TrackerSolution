using Microsoft.Extensions.Logging;
using Tracker.Services.Interfaces;
using Tracker.Services;
using Tracker.ViewModels;

namespace Tracker;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<ILocationService, LocationService>();

        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<MainView>();

        return builder.Build();
    }
}