namespace TRMDesktop.Library.Api
{
    using System.Threading.Tasks;
    using TRMDesktop.Library.Models;

    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel sale);
    }
}