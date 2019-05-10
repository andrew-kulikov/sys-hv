using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace SysHv.Server.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string CompanyName { get; set; }
        public ICollection<Client> Clients { get; set; }
    }
}
