namespace TRMDesktop.Library.Api
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using TRMDesktop.Library.Models;
    using TRMDesktopUI.Models;

    public class APIHelper : IAPIHelper
    {
        private HttpClient apiClient;

        public HttpClient ApiClient
        {
            get { return apiClient; }
        }

        private ILoggedInUserModel loggedInUserModel;

        public APIHelper(ILoggedInUserModel loggedInUserModel)
        {
            InitializeClient();
            this.loggedInUserModel = loggedInUserModel;
        }

        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(api);
            
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type","password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("/Token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }

        public async Task GetLoggedInUserInfo(string token)
        {
            apiClient.DefaultRequestHeaders.Clear();
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jsoon"));
            apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            using (HttpResponseMessage response = await apiClient.GetAsync("/api/User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
                    loggedInUserModel.CreatedDate = result.CreatedDate;
                    loggedInUserModel.EmailAddress = result.EmailAddress;   
                    loggedInUserModel.FirsName = result.FirsName;   
                    loggedInUserModel.Id = result.Id;  
                    loggedInUserModel.LastName = result.LastName;   
                    loggedInUserModel.Token = token;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);

                }
            }
        }
    }
}
