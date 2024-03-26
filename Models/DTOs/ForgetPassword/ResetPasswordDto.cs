using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.ForgetPassword
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Id is required!")]
        public string Id { get; set; }

        [Required(ErrorMessage = "New password can not be empty")]
        [MinLength(8, ErrorMessage = "Password length must be at least 8 characters!")]
        public string NewPassword { get; set; }
        
        [Required(ErrorMessage = "Token was not provided.")]
        public string SecureToken { get; set; }
        
        [Required(ErrorMessage = "Expire date was not provided")]
        public string Expiration { get; set; }
    }
}
