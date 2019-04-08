using System.Configuration;

namespace SysHv.Client.Common.Models
{
    public class ConnectionModel
    {
        public string Host { get; }
        public string Username { get; }
        public string Password { get; }

        public ConnectionModel(string host, string username, string password)
        {
            Host = host;
            Username = username;
            Password = password;
        }

        public ConnectionModel()
        {
            Host = ConfigurationManager.AppSettings["Host"];
            Username = ConfigurationManager.AppSettings["Username"];
            Password = ConfigurationManager.AppSettings["Password"];
        }
    }
}
