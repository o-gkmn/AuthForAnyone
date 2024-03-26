using Core;
using Core.Abstract;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.ForgetPassword;

namespace AuthForAnyone.Controllers
{
    [ApiController]
    [Route("api/forget-password")]
    public class ForgetPasswordController(IForgetPasswordService forgetPasswordService) : ControllerBase
    {
        private IForgetPasswordService ForgetPasswordService { get; } = forgetPasswordService;

        [HttpPost]
        public async Task<IActionResult> ForgetPasswordAsync(RequestForgetPasswordDto requestForgetPasswordDto)
        {
            await ForgetPasswordService.ForgetPasswordAsync(requestForgetPasswordDto);
            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> CreateNewPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var result = await ForgetPasswordService.CreateNewPasswordAsync(resetPasswordDto);
            return Ok(result);
        }
    }
}