using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SysHv.Server.DAL.Models;

namespace SysHv.Server.DAL
{
    public class ServerDbContext : IdentityDbContext
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options)
        {

        }
    }
}
