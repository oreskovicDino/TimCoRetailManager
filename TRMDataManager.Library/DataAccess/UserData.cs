namespace TRMDataManager.Library.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TRMDataManager.Library.Internal.DataAcsess;
    using TRMDataManager.Library.Models;

    public class UserData
    {
        public List<UserModel> GetUserById(string Id)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { Id = Id };
            var output = sql.LoadData<UserModel,dynamic>("dbo.spUserLookUp", p, "TRMData");
            return output;
        }
    }
}
