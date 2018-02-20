using System;
using System.Collections.Generic;
using System.Windows.Input;
using ProTrakr.Models;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using Realms;
using System.Linq;
using System.Collections.ObjectModel;
using MvvmHelpers;

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

        public ClientListPageViewModel(INavigationService navigationService, Realm realm)
            : base(navigationService)
        {
            _realm = realm;

            Title = "Clients";

            //var bsiLabs = new Client
            //{
            //    Name = "BSI Labs",
            //    Location = "Toronto, Ontario, Canada"
            //};
            //bsiLabs.Projects.Add(new Project { Name = "ProTrakr", StartDate = DateTime.Today });

            //var microsoft = new Client
            //{
            //    Name = "Microsoft",
            //    Location = "Redmond, Washington, USA"
            //};
            //microsoft.Projects.Add(new Project { Name = "Xamarin", StartDate = DateTime.Today });
            //microsoft.Projects.Add(new Project { Name = "Visual Studio", StartDate = DateTime.Today });

            //ClientList = new List<Client>
            //{
            //    bsiLabs,
            //    microsoft
            //};
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