namespace TRMDesktop.Library.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using System.Threading.Tasks;

    public class LoggedInUserModel : ILoggedInUserModel
    {
        public string Token { get; set; }
        public string Id { get; set; }
        public string FirsName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }

        public void ResetUserModel()
        {
            Token = "";
            Id = "";
            LastName = "";
            EmailAddress = "";
            CreatedDate = DateTime.MinValue;
        }
    }
}
