using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.UserDTOs;

namespace AuthForAnyone.Controllers
{
    [Route("api/sign")]
    [ApiController]
    public class SignController : ControllerBase
    {
        public readonly ISignService _signService;

        public SignController(ISignService signService)
        {
            _signService = signService;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignInAsync(SignInUserDto signInUserDto)
        {
            var result = await _signService.SignInAsync(signInUserDto);

            return Ok(result);
        }

        [HttpPost("sign-out")]
        public IActionResult SignOut(SignInUserDto signInUserDto)
        {
            return BadRequest();
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUpAsync(SignUpUserDto signUpUserDto)
        {
            var result = await _signService.SignUpAsync(signUpUserDto);

            return Ok(result);
        }
    }
}
