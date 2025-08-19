using System.ComponentModel.DataAnnotations;
namespace Shared.ModelsDto
{
    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

}