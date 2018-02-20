using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using ProTrakr.Configuration;
using ProTrakr.Services;
using Realms.Sync;

namespace ProTrakr.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public ICommand LoginCommand => new DelegateCommand(async() => await OnLoginCommand());

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private IRealmService _realmService;

        public LoginPageViewModel(INavigationService navigationService, IRealmService realmService) : base(navigationService)
        {
            _realmService = realmService;
        }

        private async Task OnLoginCommand()
        {
            try
            {
                var credentials = Credentials.UsernamePassword(UserName, Password, createUser: false);
                _realmService.User = await User.LoginAsync(credentials, EnvironmentConfig.Instance.AuthURL);
                _realmService.SyncConfiguration = new SyncConfiguration(_realmService.User, EnvironmentConfig.Instance.ServerURL);

                await NavigationService.NavigateAsync("ClientListPage");
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
        }
    }
}
