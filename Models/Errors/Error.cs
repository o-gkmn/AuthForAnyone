using Microsoft.AspNetCore.Http;

namespace Models.Errors;

public sealed class Error(int code, string Type, string description) : Exception(description)
{
    public int Code { get; init; } = code;
    public string Description { get; init; } = description;
    public string Type { get; init; } = Type;
}

public class UserError
{
    public static readonly Error UserAlreadyExist = new Error(StatusCodes.Status400BadRequest, "UserError.UserAlreadyExist", "This username cannot be used because it is already in use.");
    public static readonly Error UserNotFound = new Error(StatusCodes.Status400BadRequest, "UserError.UserNotFound", "This user cannot be found. Wrong email or username");
    public static readonly Error WrongPassword = new Error(StatusCodes.Status400BadRequest, "UserError.WrongPassword", "Password is not correct.");
}

public class ModelError
{
    public static readonly Error ModelNull = new Error(StatusCodes.Status400BadRequest, "ModelError.ModelNull", "The given model was null!");
    public static readonly Error EmptyPassword = new Error(StatusCodes.Status400BadRequest, "ModelError.EmptyPassword", "Password cannot be empty!");
    public static readonly Error EmptyUserName = new Error(StatusCodes.Status400BadRequest, "ModelError.EmptyUserName", "Username cannot be empty!");

}

public class TokenError
{
    public static readonly Error InvalidToken = new Error(StatusCodes.Status400BadRequest, "TokenError.InvalidToken", "The given token is invalid. Please login again.");
    public static readonly Error MissingToken = new Error(StatusCodes.Status400BadRequest, "TokenError.MissingToken", "Required token is not provided");
    public static readonly Error AccessTokenExpired = new Error(StatusCodes.Status401Unauthorized, "TokenError.AccessTokenExpired", "The access token was expired.");
    public static readonly Error RefreshTokenExpired = new Error(StatusCodes.Status401Unauthorized, "TokenError.RefreshTokenExpired", "The refresh token was expired.");
    public static readonly Error UnreadableToken = new Error(StatusCodes.Status401Unauthorized, "TokenError.UnreadableToken", "The given token is unreadable.");
    public static readonly Error FailedToGenerateToken = new Error(StatusCodes.Status500InternalServerError, "TokenError.FailedToGenerateToken", "New token generation failed.");
}

public class ServerError
{
    public static readonly Error ServerNotResponding = new Error(StatusCodes.Status503ServiceUnavailable, "ServerError.ServerNotResponding", "Server not responding.");
}

public class SessionError
{
    public static readonly Error SessionTimeExpired = new Error(StatusCodes.Status401Unauthorized, "SessionError.SessionTimeExpired", "Session time expired. Please sign in again.");
}

public class ForgetPasswordError
{
    public static readonly Error EmptyEmail = new Error(StatusCodes.Status400BadRequest, "ForgetPasswordError.EmptyEmail", "Email can not be empty!");
    public static readonly Error ResetTokenExpired = new Error(StatusCodes.Status400BadRequest, "ForgetPasswordError.ResetTokenExpired", "This is not valid request.");
    public static readonly Error RequiredFieldIsEmpty = new Error(StatusCodes.Status400BadRequest, "ForgetPasswordError.RequiredFieldIsEmpty", "Required field can not be empty!");
    public static readonly Error SamePassword = new Error(StatusCodes.Status400BadRequest, "ForgetPasswordError.SamePassword", "the new password cannot be the same as the previous one");
}