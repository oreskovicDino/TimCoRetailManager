namespace TRMDesktopUI.ViewModels
{
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TRMDesktopUI.EventModels;

    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private IEventAggregator @event;
        private SalesViewModel salsesVM;
        private SimpleContainer container;

        public ShellViewModel(IEventAggregator @event, SalesViewModel salsesVM, SimpleContainer container)
        {
            this.salsesVM = salsesVM;
            this.container = container;
            this.@event = @event;
            this.@event.Subscribe(this);

            ActivateItem(this.container.GetInstance<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(salsesVM);
        }
    }
}
