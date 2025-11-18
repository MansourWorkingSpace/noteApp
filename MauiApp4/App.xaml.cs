namespace MauiApp4
{
    public partial class App : Application
    {
        // Single shared router instance for simple MVVM routing
        public static ViewModels.RoutingViewModel Router { get; } = new ViewModels.RoutingViewModel();

        // Service provider set during app startup so pages/ViewModels can resolve services.
        public static System.IServiceProvider? Services { get; internal set; }

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
