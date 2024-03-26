namespace Models.DTOs.ForgetPassword
{
    public class ResetPasswordDto
    {
        public string Id { get; set; }
        public string NewPassword { get; set; }
        public string SecureToken { get; set; }
        public string Expiration { get; set; }
    }
}
