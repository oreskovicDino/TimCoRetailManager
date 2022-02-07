namespace TRMDesktopUI.ViewModels
{
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Dynamic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    using TRMDesktop.Library.Api;
    using TRMDesktop.Library.Models;

    public class UserDisplayViewModel : Screen
    {
        private StatusInfoViewModel status;
        private IWindowManager window;
        private IUserEndpoint userEndpoint;
        private BindingList<UserModel> users;
        private UserModel selectedUser;
        private string selectedUserName;
        private BindingList<string> userRoles = new BindingList<string>();
        private BindingList<string> availableRoles = new BindingList<string>();
        private string selectedUserRole;
        private string selectedAvailableRole;

        public BindingList<UserModel> Users
        {
            get { return users; }
            set
            {
                users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        public UserModel SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                SelectedUserName = value.Email;
                UserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                LoadRoles();
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }

        public string SelectedUserName
        {
            get { return selectedUserName; }
            set
            {
                selectedUserName = value;
                NotifyOfPropertyChange(() => SelectedUserName);
            }
        }

        public BindingList<string> UserRoles
        {
            get { return userRoles; }
            set
            {
                userRoles = value;
                NotifyOfPropertyChange(() => UserRoles);

            }
        }

        public BindingList<string> AvailableRoles
        {
            get { return availableRoles; }
            set
            {
                availableRoles = value;
                NotifyOfPropertyChange(() => AvailableRoles);

            }
        }


        public string SelectedUserRole
        {
            get { return selectedUserRole; }
            set 
            {
                selectedUserRole = value;
                NotifyOfPropertyChange(() => SelectedUserRole);
            }
        }

        public string SelectedAvailableRole
        {
            get { return selectedAvailableRole; }
            set
            {
                selectedAvailableRole = value;
                NotifyOfPropertyChange(() => SelectedAvailableRole);
            }
        }

        public UserDisplayViewModel(
            StatusInfoViewModel status,
            IWindowManager window,
            IUserEndpoint userEndpoint
            )
        {
            this.status = status;
            this.window = window;
            this.userEndpoint = userEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadUsers();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLopcation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                if (ex.Message == "Unauthorized")
                {
                    status.UpdateMessage("Unauthorize Access!", "You do not have permission to interact wiht the Sale Form.");
                    window.ShowDialog(status, null, settings);
                }
                else
                {
                    status.UpdateMessage("Fatal Exception!", ex.Message);
                    window.ShowDialog(status, null, settings);
                }
                TryClose();
            }
        }

        private async Task LoadUsers()
        {
            var userList = await userEndpoint.GetAll();
            Users = new BindingList<UserModel>(userList);
        }

        private async Task LoadRoles()
        {
            var roles = await userEndpoint.GetAllRoles();

            foreach (var role in roles)
            {
                if (userRoles.IndexOf(role.Value) < 0)
                {
                    AvailableRoles.Add(role.Value);
                }
            }
        }

        public async Task AddSelectedRole()
        {
           await userEndpoint.AddUserToRole(SelectedUser.Id, SelectedAvailableRole);

            UserRoles.Add(SelectedAvailableRole);
            AvailableRoles.Remove(SelectedAvailableRole);

        }  
        
        public async Task RemoveSelectedRole()
        {
            await userEndpoint.RemoveUserFromRole(SelectedUser.Id, SelectedUserRole);

            AvailableRoles.Add(SelectedUserRole);
            UserRoles.Remove(SelectedUserRole);
        }
    }
}
