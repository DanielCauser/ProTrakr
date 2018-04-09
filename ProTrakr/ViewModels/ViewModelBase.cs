using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Navigation;
using ProTrakr.Services;

namespace ProTrakr.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected readonly IUtilityService UtilityService;
        protected readonly INavigationService NavigationService;
        protected readonly IErrorManagementService ErrorManagementService;

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        // Credit to Chase Florell: https://chaseflorell.github.io/xamarin/2017/12/07/isbusy-like-a-boss/
        private static readonly Guid DefaultTracker = default(Guid);
        private readonly IList<Guid> _busyLocks = new List<Guid>();
        public bool IsBusy
        {
            get => _busyLocks.Any();
            set
            {
                if (value && !_busyLocks.Contains(DefaultTracker))
                {
                    _busyLocks.Add(DefaultTracker);
                    RaisePropertyChanged(nameof(IsBusy));
                }

                if (!value && _busyLocks.Contains(DefaultTracker))
                {
                    _busyLocks.Remove(DefaultTracker);
                    RaisePropertyChanged(nameof(IsBusy));
                }
            }
        }

        protected BusyHelper Busy() => new BusyHelper(this);

        public ViewModelBase(INavigationService navigationService, IUtilityService utilityService)
        {
            NavigationService = navigationService;
            UtilityService = utilityService;
            ErrorManagementService = utilityService.ErrorManagementService;
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }

        /// <summary>
        ///     The default exception handling for RunSafe or RunSafeAsync
        ///     If you choose to extend this, calling <code>base.OnError(ex)</code> will send the error to the
        ///     <see cref="IErrorManagementService" />. 
        ///     Known Exceptions are handled by ErrorManagementService which maps to the appropriate resource key 
        ///     and displays a dialog containing the string in the resource entry.
        /// </summary>
        /// <param name="ex">The thrown exception</param>
        /// <remarks>Swallows <see cref="TaskCanceledException"/>s</remarks>
        protected virtual void OnError(Exception ex)
        {
            switch (ex)
            {
                case TaskCanceledException _: /*Gulp*/
                    return;
                case AggregateException _:
                    ErrorManagementService.HandleError(ex.InnerException);
                    break;
                default:
                    ErrorManagementService.HandleError(ex);
                    break;
            }
        }

        /// <summary>
        ///     Wrap your potentially volatile calls with RunSafe to have any exceptions automagically handled for you
        /// </summary>
        /// <param name="action">Action to run</param>
        protected void RunSafe(Action action) => RunSafe(action, OnError);

        /// <summary>
        ///     Wrap your potentially volatile calls with RunSafe to have any exceptions automagically handled for you
        /// </summary>
        /// <param name="action">Action to run</param>
        /// <param name="handleErrorAction">(optional) Custom Action to invoke with the thrown Exception</param>
        protected void RunSafe(Action action, Action<Exception> handleErrorAction)
        {
            try
            {
                action.Invoke();
            }

            catch (Exception ex)
            {
                handleErrorAction?.Invoke(ex);
            }
        }

        /// <summary>
        ///     Wrap your potentially volatile calls with RunSafeAsync to have any exceptions automagically handled for you
        /// </summary>
        /// <param name="task">Task to run</param>
        protected Task RunSafeAsync(Func<Task> task) => RunSafeAsync(task, OnError);

        /// <summary>
        ///     Wrap your potentially volatile calls with RunSafeAsync to have any exceptions automagically handled for you
        /// </summary>
        /// <param name="task">Task to run</param>
        /// <param name="handleErrorAction">(optional) Custom Action to invoke with the thrown Exception</param>
        protected async Task RunSafeAsync(Func<Task> task, Action<Exception> handleErrorAction)
        {
            try
            {
                await task().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                handleErrorAction?.Invoke(ex);
            }
        }

        /// <summary>
        ///     Wrap your potentially volatile calls with RunSafeAsync to have any exceptions automagically handled for you
        /// </summary>
        /// <typeparam name="T">Type of the returned object</typeparam>
        /// <param name="task">Task to run</param>
        protected Task<T> RunSafeAsync<T>(Func<Task<T>> task) => RunSafeAsync(task, OnError);

        /// <summary>
        ///     Wrap your potentially volatile calls with RunSafeAsync to have any exceptions automagically handled for you
        /// </summary>
        /// <typeparam name="T">Type of the returned object</typeparam>
        /// <param name="task">Task to run</param>
        /// <param name="handleErrorAction">(optional) Custom Action to invoke with the thrown Exception</param>
        protected async Task<T> RunSafeAsync<T>(Func<Task<T>> task, Action<Exception> handleErrorAction)
        {
            try
            {
                return await task().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                handleErrorAction?.Invoke(ex);
            }
            return default(T);
        }

        // Credit to Chase Florell: https://chaseflorell.github.io/xamarin/2017/12/07/isbusy-like-a-boss/
        protected sealed class BusyHelper : IDisposable
        {
            private readonly ViewModelBase _viewModel;
            private readonly Guid _tracker;

            public BusyHelper(ViewModelBase viewModel)
            {
                _viewModel = viewModel;
                _tracker = new Guid();
                _viewModel._busyLocks.Add(_tracker);
                _viewModel.RaisePropertyChanged(nameof(IsBusy));
            }

            public void Dispose()
            {
                _viewModel._busyLocks.Remove(_tracker);
                _viewModel.RaisePropertyChanged(nameof(IsBusy));
            }
        }
    }
}
