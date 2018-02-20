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
using Plugin.Connectivity;
using MonkeyCache.SQLite;
using MonkeyCache;

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
        private string _url = "http://demo7345493.mockable.io/api/client";

        public ClientListPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Clients";
        }

        public async Task OnLoadCommand()
        {
            var clients = await GetData();
            ClientList.ReplaceRange(clients?.OrderBy(c => c.Name));
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
                var json = await _httpClient.GetStringAsync(_url);
                clients = JsonConvert.DeserializeObject<List<Client>>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return clients;
        }

        //private async Task<List<Client>> GetData()
        //{
        //    List<Client> clients = new List<Client>();
        //    try
        //    {
        //        //Dev handle online/offline scenario
        //        if (!CrossConnectivity.Current.IsConnected)
        //        {
        //            return Barrel.Current.Get<List<Client>>(key: _url);
        //        }

        //        //Dev handles checking if cache is expired
        //        if (!Barrel.Current.IsExpired(key: _url))
        //        {
        //            return Barrel.Current.Get<List<Client>>(key: _url);
        //        }

        //        var json = await _httpClient.GetStringAsync(_url);
        //        clients = JsonConvert.DeserializeObject<List<Client>>(json);

        //        //Saves the cache and pass it a timespan for expiration
        //        Barrel.Current.Add(key: _url, data: clients, expireIn: TimeSpan.FromDays(1));
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    } 
        //    return clients;
        //}

        //private async Task<List<Client>> GetData()
        //{
        //    List<Client> clients = new List<Client>();
        //    try
        //    {
        //        //Dev handle online/offline scenario
        //        if (!CrossConnectivity.Current.IsConnected)
        //        {
        //            return Barrel.Current.Get<List<Client>>(key: _url);
        //        }

        //        var result = await HttpCache.Current.GetCachedAsync(Barrel.Current, _url, TimeSpan.FromSeconds(60), TimeSpan.FromDays(1));
        //        clients = JsonConvert.DeserializeObject<List<Client>>(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //    return clients;
        //}

        public override async void OnNavigatingTo(NavigationParameters parameters)
        {
            parameters.TryGetValue("Client", out Client client);

            var clients = await GetData();

            if (client != null)
            {
                clients.Add(client);
            }

            ClientList.ReplaceRange(clients?.OrderBy(c => c.Name));
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}