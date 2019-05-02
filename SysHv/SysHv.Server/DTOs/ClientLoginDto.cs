using Newtonsoft.Json;

namespace SysHv.Server.DTOs
{
    public class ClientLoginDto
    {
        [JsonProperty("email")] public string Email { get; set; }

        [JsonProperty("passwordHash")] public string PasswordHash { get; set; }

        [JsonProperty("id")] public int Id { get; set; }
    }
}