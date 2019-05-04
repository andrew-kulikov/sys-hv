using System.Configuration;
using SysHv.Client.Common.DTOs;

namespace SysHv.Client.WinService.Helpers
{
    public static class ConfigurationHelper
    {
        public static string Email { get; } = ConfigurationManager.AppSettings["Email"];
        public static string PasswordHash { get; } = ConfigurationManager.AppSettings["PasswordHash"];
        public static int Id { get; } = int.Parse(ConfigurationManager.AppSettings["Id"]);
        public static string ServerAddress { get; } = ConfigurationManager.AppSettings["ServerAddress"];
        public static int ReconnectionInterval { get; } = int.Parse(ConfigurationManager.AppSettings["ReconnectionInterval"]);

        public static ClientLoginDto LoginDto => new ClientLoginDto
        {
            Email = Email,
            PasswordHash = PasswordHash,
            Id = Id
        };
    }
}