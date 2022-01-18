namespace TRMDesktop.Library.Models
{
    using System;

    public interface ILoggedInUserModel
    {
        DateTime CreatedDate { get; set; }
        string EmailAddress { get; set; }
        string FirsName { get; set; }
        string Id { get; set; }
        string LastName { get; set; }
        string Token { get; set; }

        void LogOffUser();
    }
}