using System.Windows.Input;
using ProTrakr.Models;
using Prism.Commands;
using Prism.Navigation;

namespace ProTrakr.ViewModels
{
    public class ClientDetailPageViewModel : ViewModelBase
    {
        public ICommand SaveCommand => new DelegateCommand(OnSaveCommand);
        public ICommand DetailCommand => new DelegateCommand(OnDetailCommand);
        private Client _client;

        public Client Client
        {
            get => _client;
            set => SetProperty(ref _client, value);
        }

        public ClientDetailPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            parameters.TryGetValue("Client", out Client client);
            Client = client ?? new Client();
            Title = Client.Name;
        }

        private void OnSaveCommand()
        {
            NavigationService.GoBackAsync(new NavigationParameters { { "Client", Client } });
        }

        private void OnDetailCommand()
        {
            
        }
    }
}