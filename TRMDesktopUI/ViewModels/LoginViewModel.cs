﻿namespace TRMDesktopUI.ViewModels
{
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TRMDesktop.Library.Api;
    using TRMDesktopUI.EventModels;
    using TRMDesktopUI.Helpers;

    public class LoginViewModel : Screen
    {
        private IAPIHelper helper;
        private IEventAggregator eventAggregator;
        private string _userName;
        private string _password;

        public LoginViewModel(IAPIHelper helper, IEventAggregator eventAggregator)
        {
            this.helper = helper;
            this.eventAggregator = eventAggregator;
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
           }
        }

        private bool isErrorVisible;

        public bool IsErrorVisible
        {
            get { 
            bool output = false;
                if (ErrorMessage?.Length > 0)
                {
                    output = true;
                }

                return output;
            }
            set {
                NotifyOfPropertyChange(() => IsErrorVisible);
                isErrorVisible = value; 
            }
        }

        private string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set {
                errorMessage = value; 
                NotifyOfPropertyChange(() => IsErrorVisible );
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }


        public bool CanLogIn
        {
            get{
                bool output = false;

                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    output = true;
                }
                return output;
                //Capture more information about the user.

            }
        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = "";

                var result = await helper.Authenticate(UserName, Password);
                await helper.GetLoggedInUserInfo(result.Access_Token);

                eventAggregator.PublishOnUIThread(new LogOnEvent());
                
            }
            catch (Exception ex )
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
