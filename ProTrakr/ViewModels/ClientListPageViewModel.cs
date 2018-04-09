using System.Windows.Input;
using ProTrakr.Models;
using Prism.Commands;
using Prism.Navigation;
using System.Linq;
using MvvmHelpers;
using System.Threading.Tasks;
using ProTrakr.Services;

namespace ProTrakr.ViewModels
{
    public class ClientListPageViewModel : ViewModelBase
    {
        private readonly ClientDataService _clientDataService;
        private ICommand _newCommand;
        public ICommand NewCommand => _newCommand = _newCommand ?? new DelegateCommand(OnNewCommand);
        private ICommand _detailCommand;
        public ICommand DetailCommand => _detailCommand = _detailCommand ?? new DelegateCommand<Client>(OnDetailCommand);
        private ICommand _refreshCommand;
        public ICommand RefreshCommand => _refreshCommand = _refreshCommand ?? new DelegateCommand(async () => await OnLoadCommand());
        public ObservableRangeCollection<Client> ClientList { get; } = new ObservableRangeCollection<Client>();

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        
        public ClientListPageViewModel(INavigationService navigationService, IUtilityService utilityService, ClientDataService clientDataService)
            : base(navigationService, utilityService)
        {
            _clientDataService = clientDataService;
            Title = "Clients";
        }

        public async Task OnLoadCommand()
        {
            var clients = await RunSafeAsync(() => _clientDataService.GetClientList());
            ClientList.ReplaceRange(clients.OrderBy(c => c.Name));
            IsRefreshing = false;
        }

        public void OnNewCommand()
        {
            NavigationService.NavigateAsync("ClientDetailPage");
        }

        public void OnDetailCommand(Client item)
        {
            NavigationService.NavigateAsync("ClientDetailPage", new NavigationParameters { { "Client", item } });
        }

        public override async void OnNavigatingTo(NavigationParameters parameters)
        {
            parameters.TryGetValue("Client", out Client client);

            var clients = await RunSafeAsync(() => _clientDataService.GetClientList());
            if (client != null)
            {
                clients.Add(client);
            }

            if (clients != null)
            {
                ClientList.ReplaceRange(clients.OrderBy(c => c.Name));
            }
        }
    }
}