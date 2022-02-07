namespace TRMDataManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ApplicationUserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public Dictionary<string,string> Roles { get; set; } = new Dictionary<string,string>();
    }
}