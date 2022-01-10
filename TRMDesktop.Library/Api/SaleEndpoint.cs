namespace TRMDesktop.Library.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using TRMDesktop.Library.Models;

    public class SaleEndpoint : ISaleEndpoint
    {
        private IAPIHelper apiHelper;

        public SaleEndpoint(IAPIHelper apiHelper)
        {
            this.apiHelper = apiHelper;
        }

        public async Task PostSale(SaleModel sale)
        {
            using (HttpResponseMessage responseMessage = await apiHelper.ApiClient.PostAsJsonAsync("/api/Sale", sale))
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    // Log successful call?
                }
                else
                {
                    throw new Exception(responseMessage.ReasonPhrase);
                }
            }
        }
    }
}
