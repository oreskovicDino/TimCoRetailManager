namespace TRMDataManager.Controllers
{
    using Microsoft.AspNet.Identity;
    using System.Collections.Generic;
    using System.Web.Http;
    using TRMDataManager.Library.DataAccess;
    using TRMDataManager.Library.Models;

    [Authorize]
    public class SaleController : ApiController
    {
        [Authorize(Roles = "Cashier")]
        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData();
            data.SaveSale(sale,RequestContext.Principal.Identity.GetUserId());
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("GetSalesReport")]
        public List<SaleReportModel> GetSalesReport()
        {
            SaleData data = new SaleData();
            return data.GetSaleReport();
        }
    }
}