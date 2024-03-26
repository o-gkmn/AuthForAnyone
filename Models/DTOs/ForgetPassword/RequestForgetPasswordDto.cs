using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.ForgetPassword;

public class RequestForgetPasswordDto
{
    [Required(ErrorMessage = "Email cannot be empty")]
    public string Email { get; set; }
}
