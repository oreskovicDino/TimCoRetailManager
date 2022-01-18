namespace TRMDataManager.Library.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;
    using TRMDataManager.Library.Internal.DataAcsess;
    using TRMDataManager.Library.Models;

    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();

            return sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TRMData");
        }

        public ProductModel GetProdcutById(int productId)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = productId }, "TRMData").FirstOrDefault();
            return output;
        }
    }
}
