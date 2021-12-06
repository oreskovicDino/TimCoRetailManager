namespace TRMDesktopUI.ViewModels
{
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TRMDesktopUI.Helpers;

    public class LoginViewModel : Screen
    {
        private IAPIHelper helper;
        private string _userName;
        private string _password;

        public LoginViewModel(IAPIHelper helper)
        {
            this.helper = helper;
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
            }
        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = "";

                var result = await helper.Authenticate(UserName, Password);
            }
            catch(Exception ex )
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
