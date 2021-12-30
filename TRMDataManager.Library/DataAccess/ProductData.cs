namespace TRMDataManager.Library.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TRMDataManager.Library.Internal.DataAcsess;
    using TRMDataManager.Library.Models;

    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();

            return sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new {}, "TRMData");
        }
    }
}
