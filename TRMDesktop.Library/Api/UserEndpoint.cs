namespace TRMDesktop.Library.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using TRMDesktop.Library.Models;

    public class UserEndpoint : IUserEndpoint
    {
        private IAPIHelper helper;

        public UserEndpoint(IAPIHelper helper)
        {
            this.helper = helper;
        }

        public async Task<List<UserModel>> GetAll()
        {
            using (HttpResponseMessage response = await helper.ApiClient.GetAsync("/api/User/Admin/GetAllUsers"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<UserModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<Dictionary<string,string>> GetAllRoles()
        {
            using (HttpResponseMessage response = await helper.ApiClient.GetAsync("/api/User/Admin/GetAllRoles"))
            {
                if (response.IsSuccessStatusCode)
                {
                  return await response.Content.ReadAsAsync<Dictionary<string, string>>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task AddUserToRole(string userId, string roleName)
        {
            var data = new { userId, roleName };


            using (HttpResponseMessage response = await helper.ApiClient.PostAsJsonAsync("/api/User/Admin/AddRole", data))
            {
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task RemoveUserFromRole(string userId, string roleName)
        {
            var data = new { userId, roleName };


            using (HttpResponseMessage response = await helper.ApiClient.PostAsJsonAsync("/api/User/Admin/RemoveRole", data))
            {
                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
