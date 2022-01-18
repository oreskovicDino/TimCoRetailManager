namespace TRMDataManager.Controllers
{
    using Microsoft.AspNet.Identity;
    using System.Web.Http;
    using TRMDataManager.Library.DataAccess;
    using TRMDataManager.Library.Models;

    [Authorize]
    public class SaleController : ApiController
    {
        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData();
            data.SaveSale(sale,RequestContext.Principal.Identity.GetUserId());
        }

    }
}