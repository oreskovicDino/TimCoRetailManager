namespace TRMDesktopUI.ViewModels
{
    using Caliburn.Micro;
    using TRMDesktopUI.EventModels;

    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private IEventAggregator @event;
        private SalesViewModel salsesVM;

        public ShellViewModel(IEventAggregator @event, SalesViewModel salsesVM)
        {
            this.salsesVM = salsesVM;
            this.@event = @event;
            this.@event.Subscribe(this);

            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(salsesVM);
        }
    }
}
