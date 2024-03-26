using Identity.Models;

namespace Identity.Interfaces;

public interface IPersonaManager
{
    Task<UserEntity> FindByEmailAsync(string email);
    Task<UserEntity> FindUserByUserNameAsync(string userName);
    Task<string> CreateResetPasswordTokenAsync(string email);
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    Task<string> FindIdByEmailAsync(string email);
}