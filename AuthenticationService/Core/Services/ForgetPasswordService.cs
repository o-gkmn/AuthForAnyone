using Identity.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Models.DTOs.ForgetPassword;
using Models.Errors;

namespace Core;

public class ForgetPasswordService(IPersonaManager personaManager) : IForgetPasswordService
{
    public async Task ForgetPasswordAsync(RequestForgetPasswordDto requestForgetPasswordDto)
    {
        if (string.IsNullOrEmpty(requestForgetPasswordDto.Email))
        {
            throw ForgetPasswordError.EmptyEmail;
        }

        var user = await personaManager.FindByEmailAsync(requestForgetPasswordDto.Email);
        var resetToken = await personaManager.CreateResetPasswordTokenAsync(requestForgetPasswordDto.Email);
        var resetLink = await CreatePasswordResetLinkAsync(resetToken, requestForgetPasswordDto.Email);
        SendEmail(resetLink, user.Email, user.UserName);
    }

    public async Task<bool> CreateNewPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        if (string.IsNullOrEmpty(resetPasswordDto.Id) ||
            string.IsNullOrEmpty(resetPasswordDto.NewPassword) ||
            string.IsNullOrEmpty(resetPasswordDto.SecureToken) ||
            string.IsNullOrEmpty(resetPasswordDto.Expiration))
        {
            throw ForgetPasswordError.RequiredFieldIsEmpty;
        }

        if (IsResetTokenExpire(resetPasswordDto.Expiration)) throw ForgetPasswordError.ResetTokenExpired;

        var result = await personaManager.ResetPasswordAsync(resetPasswordDto.Id, resetPasswordDto.SecureToken, resetPasswordDto.NewPassword);
        return result;
    }

    private void SendEmail(string resetLink, string mail, string name)
    {
        var email = new MimeMessage();
        IConfiguration configuration = CreateInstanceOfIConfiguration();
        var smtpCredentials = configuration.GetSection("SmtpCredentials");

        email.From.Add(new MailboxAddress("Özgür", "ozgur.gokmen735@gmail.com"));
        email.To.Add(new MailboxAddress(name, mail));

        email.Subject = "Reset Password For Dev Env";
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = $"<b>Follow the link below for reset password</b><br><a href=\"{resetLink}\">Linke gitmek için tıklayın</a>"
        };

        using (var smtp = new SmtpClient())
        {
            smtp.Connect("smtp.gmail.com", 587, false);
            smtp.Authenticate(smtpCredentials["UserName"], smtpCredentials["Password"]);

            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }

    private async Task<string> CreatePasswordResetLinkAsync(string token, string email)
    {
        var baseUrl = "https://yourwebsite.com/account/reset-password";
        var userId = await personaManager.FindIdByEmailAsync(email);

        var queryParams = new Dictionary<string, string>()
        {
            { "id", userId },
            { "token", token },
            { "expiration", Uri.EscapeDataString(DateTime.UtcNow.AddHours(24).ToString()) }
        };

        return baseUrl + "?" + string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
    }

    private bool IsResetTokenExpire(string expiration)
    {
        var expirationDateTime = DateTime.Parse(Uri.UnescapeDataString(expiration));
        if (expirationDateTime < DateTime.Now) return true;
        return false;
    }

    private IConfiguration CreateInstanceOfIConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(ApplicationDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    }

    private string ApplicationDirectory()
    {
        var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
        var appRoot = Path.GetDirectoryName(location);
        return appRoot;
    }
}
