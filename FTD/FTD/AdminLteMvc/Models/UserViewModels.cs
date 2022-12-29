using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AdminLteMvc.Models
{
    public class UserViewModels
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public ICollection<AspNetRoles> Role { get; set; }
        public string RoleId { get; set; }

        public string Employee { get; set; }
        public string UserName { get; set; }

    }
}