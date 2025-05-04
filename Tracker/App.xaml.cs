using Tracker.ViewModels;

namespace Tracker;

public partial class App : Application
{

    private readonly MainView view;

    public App(MainView view)
    {
        InitializeComponent();
        this.view = view;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new NavigationPage(view));
    }
}