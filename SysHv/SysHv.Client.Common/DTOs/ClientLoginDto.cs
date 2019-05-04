namespace SysHv.Client.Common.DTOs
{
    public class ClientLoginDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int Id { get; set; }
    }
}