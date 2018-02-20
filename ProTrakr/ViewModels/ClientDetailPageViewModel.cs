using System.Windows.Input;
using ProTrakr.Models;
using Prism.Commands;
using Prism.Navigation;
using Realms;

namespace ProTrakr.ViewModels
{
    public class ClientDetailPageViewModel : ViewModelBase
    {
        public ICommand SaveCommand => new DelegateCommand(OnSaveCommand);
        public ICommand DetailCommand => new DelegateCommand(OnDetailCommand);
        private Client _client;
        private readonly Realm _realm;

        public Client Client
        {
            get => _client;
            set => SetProperty(ref _client, value);
        }

        public ClientDetailPageViewModel(INavigationService navigationService, Realm realm)
            : base(navigationService)
        {
            _realm = realm;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            parameters.TryGetValue("Client", out Client client);
            Client = client ?? new Client();
            Title = Client.Name;
        }

        private void OnSaveCommand()
        {
            _realm.Write(() =>
            {
                _realm.Add(Client);
            });
            NavigationService.GoBackAsync(new NavigationParameters { { "Client", Client } });
        }

        private void OnDetailCommand()
        {
            
        }
    }
}