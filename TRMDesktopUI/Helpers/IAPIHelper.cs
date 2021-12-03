namespace TRMDesktopUI.Helpers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using TRMDesktopUI.Models;

    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}