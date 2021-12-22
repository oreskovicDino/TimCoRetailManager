namespace TRMDesktop.Library.Api
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using TRMDesktopUI.Models;

    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}