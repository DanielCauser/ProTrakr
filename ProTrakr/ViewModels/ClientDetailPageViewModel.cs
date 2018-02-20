using System.Windows.Input;
using ProTrakr.Models;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using Realms.Sync;
using System.Threading.Tasks;
using ProTrakr.Services;

namespace ProTrakr.ViewModels
{
    public class ClientDetailPageViewModel : ViewModelBase
    {
        public ICommand SaveCommand => new DelegateCommand(async () => await OnSaveCommand());
        public ICommand DetailCommand => new DelegateCommand(OnDetailCommand);
        private Client _client;
        private readonly Realm _realm;

        public Client Client
        {
            get => _client;
            set => SetProperty(ref _client, value);
        }

        public ClientDetailPageViewModel(INavigationService navigationService, IRealmService realmService)
            : base(navigationService)
        {
            _realm = realmService.GetInstance();
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            parameters.TryGetValue("Client", out Client client);
            Client = client ?? new Client();
            Title = Client.Name;
        }

        private async Task OnSaveCommand()
        {
            _realm.Write(() =>
            {
                _realm.Add(Client);
            });
            var session = _realm.GetSession();
            await session.WaitForUploadAsync();
            await NavigationService.GoBackAsync(new NavigationParameters { { "Client", Client } });
        }

        private void OnDetailCommand()
        {
            
        }
    }
}