using ProTrakr.Views;
using Prism;
using Prism.Ioc;
using Prism.Autofac;
using Prism.Logging;
using Prism.Services;
using ProTrakr.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ProTrakr
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/ClientListPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILoggerFacade, ConsoleLogger>();
            containerRegistry.Register<IDeviceService, DeviceService>();
            containerRegistry.Register<IErrorManagementService, ErrorManagementService>();
            containerRegistry.Register<IUtilityService, UtilityService>();

            containerRegistry.Register<ClientDataService>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<ClientListPage>();
            containerRegistry.RegisterForNavigation<ClientDetailPage>();
        }
    }
}
