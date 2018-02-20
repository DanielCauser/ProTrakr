using System;
using System.Collections.Generic;
using System.Windows.Input;
using ProTrakr.Models;
using Prism.Commands;
using Prism.Navigation;
using System.Linq;
using MvvmHelpers;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ProTrakr.ViewModels
{
    public class ClientListPageViewModel : ViewModelBase, IDisposable
    {
        public ICommand NewCommand => new DelegateCommand<Client>(OnNewCommand);
        public ICommand DetailCommand => new DelegateCommand<Client>(OnNewCommand);
        public ICommand RefreshCommand => new DelegateCommand(async () => await OnLoadCommand());
        public ObservableRangeCollection<Client> ClientList { get; } = new ObservableRangeCollection<Client>();

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private HttpClient _httpClient = new HttpClient();

        public ClientListPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Clients";
        }

        public async Task OnLoadCommand()
        {
            var clients = await GetData();
            ClientList.ReplaceRange(clients.OrderBy(c => c.Name));
            IsRefreshing = false;
        }

        public void OnNewCommand(Client item)
        {
            NavigationService.NavigateAsync("ClientDetailPage", new NavigationParameters { { "Client", item } });
        }

        private async Task<List<Client>> GetData()
        {
            List<Client> clients = new List<Client>();
            try
            {
                var result = await _httpClient.GetStringAsync("http://demo7345493.mockable.io/api/client");
                clients = JsonConvert.DeserializeObject<List<Client>>(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            } 
            return clients;
        }

        public override async void OnNavigatingTo(NavigationParameters parameters)
        {
            parameters.TryGetValue("Client", out Client client);

            var clients = await GetData();

            if (client != null)
            {
                clients.Add(client);
            }

            ClientList.ReplaceRange(clients.OrderBy(c => c.Name));
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}