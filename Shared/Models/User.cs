using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Client";
        public string UserType { get; set; } = "User";
    }
}