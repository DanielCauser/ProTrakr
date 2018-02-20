using System.Windows.Input;
using ProTrakr.Models;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using System.Linq;
using MvvmHelpers;
using ProTrakr.Services;

namespace ProTrakr.ViewModels
{
    public class ClientListPageViewModel : ViewModelBase
    {
        private readonly Realm _realm;

        public ICommand NewCommand => new DelegateCommand<Client>(OnNewCommand);
        public ICommand DetailCommand => new DelegateCommand<Client>(OnNewCommand);
        public ICommand RefreshCommand => new DelegateCommand(OnLoadCommand);
        public ObservableRangeCollection<Client> ClientList { get; } = new ObservableRangeCollection<Client>();

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ClientListPageViewModel(INavigationService navigationService, IRealmService realmService)
            : base(navigationService)
        {
            _realm = realmService.GetInstance();

            Title = "Clients";

            LoadData();
        }

        private void LoadData()
        {
            var clients = _realm.All<Client>();
            ClientList.AddRange(clients);
        }

        public void OnLoadCommand()
        {
            var clients = _realm.All<Client>();
            ClientList.ReplaceRange(clients);
            IsRefreshing = false;
        }

        public void OnNewCommand(Client item)
        {
            NavigationService.NavigateAsync("ClientDetailPage", new NavigationParameters { { "Client", item } });
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            parameters.TryGetValue("Client", out Client client);

            if (client == null) return;

            var clients = ClientList.ToList();
            clients.Add(client);
            ClientList.ReplaceRange(clients.OrderBy(c => c.Name));
        }
    }
}