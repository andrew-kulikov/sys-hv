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
        public new DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientSensor> ClientSensors { get; set; }
        public DbSet<SubSensor> SubSensors { get; set; }
        public DbSet<ClientLog> ClientLogs { get; set; }
        public DbSet<SensorLog> SensorLogs { get; set; }
        public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ClientSensor>()
                .HasOne(s => s.Sensor)
                .WithMany(s => s.ClientSensors);

            builder.Entity<Sensor>()
                .HasMany(s => s.ClientSensors)
                .WithOne(s => s.Sensor);

            builder.Entity<Sensor>()
                .HasMany(s => s.SubSensors)
                .WithOne(ss => ss.Sensor);

            base.OnModelCreating(builder);
        }
    }
}
