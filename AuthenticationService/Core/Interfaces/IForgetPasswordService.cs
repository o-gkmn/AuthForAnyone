using Models.DTOs.ForgetPassword;

namespace Core;

public interface IForgetPasswordService
{
    Task ForgetPasswordAsync(RequestForgetPasswordDto requestForgetPasswordDto);
    Task<bool> CreateNewPasswordAsync(ResetPasswordDto resetPasswordDto);
}
