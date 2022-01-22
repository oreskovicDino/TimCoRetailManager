namespace TRMDesktopUI.ViewModels
{
    using Caliburn.Micro;
    using TRMDesktop.Library.Api;
    using TRMDesktop.Library.Models;
    using TRMDesktopUI.EventModels;

    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private IEventAggregator @event;
        private SalesViewModel salsesVM;
        private ILoggedInUserModel user;
        private IAPIHelper aPIHelper;

        public ShellViewModel(IEventAggregator @event, SalesViewModel salsesVM, ILoggedInUserModel user, IAPIHelper aPIHelper)
        {
            this.salsesVM = salsesVM;
            this.user = user;
            this.aPIHelper = aPIHelper;
            this.@event = @event;
            this.@event.Subscribe(this);

            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(salsesVM);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void ExitApplication()
        {
            TryClose();
        }

        private bool isLoggedIn;

        public bool IsLoggedIn
        {
            get
            {
                bool output = false;

                if (string.IsNullOrWhiteSpace(user.Token) == false )
                {
                    output = true;
                }

                return output;
            }
            set
            {
                NotifyOfPropertyChange(() => IsLoggedIn);
                isLoggedIn = value;
            }
        }

        public void LogOut()
        {
            user.ResetUserModel();
            aPIHelper.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
