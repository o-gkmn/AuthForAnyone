using Identity.Interfaces;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Models.Errors;

namespace Identity.Services;

public class PersonaManager(UserManager<UserEntity> userManager) : IPersonaManager
{
    public async Task<UserEntity> FindUserByUserNameAsync(string userName)
    {
        var user = await userManager.FindByNameAsync(userName);
        return user ?? throw UserError.UserNotFound;
    }

    public async Task<UserEntity> FindByEmailAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user ?? throw UserError.UserNotFound;
    }

    public async Task<string> CreateResetPasswordTokenAsync(string email)
    {
        var user = await FindByEmailAsync(email);
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        return token;
    }

    public async Task<bool> ResetPasswordAsync(string id, string token, string newPassword)
    {
        var user = await userManager.FindByIdAsync(id);
        var newPasswordHash = userManager.PasswordHasher.HashPassword(user, newPassword);

        var identityResult = await userManager.ResetPasswordAsync(user, token, newPassword);

        var identityError = identityResult.Errors.FirstOrDefault();
        if (identityError != null) throw new Error(Convert.ToInt32(identityError.Code), "IdentityError", identityError.Description);
        
        return identityResult.Succeeded;
    }

    public async Task<string> FindIdByEmailAsync(string email)
    {
        var user = await FindByEmailAsync(email);
        return user.Id;
    }
}