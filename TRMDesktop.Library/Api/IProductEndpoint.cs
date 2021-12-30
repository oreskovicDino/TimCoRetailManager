namespace TRMDesktop.Library.Api
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TRMDesktop.Library.Models;

    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}