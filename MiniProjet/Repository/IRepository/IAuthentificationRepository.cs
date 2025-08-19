using MiniProjet.ModelsDto;
using Shared.ModelsDto;
using System.Threading.Tasks;

namespace MiniProjet.Repository.IRepository
{
    public interface IAuthentificationRepository
    {
        Task<string> LoginAsync(LoginUserDto loginUserDto);
        Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<bool> UserExistsAsync(string email, string username);
        Task<UserDto> ValidateUserAsync(string email, string password);

    }
}