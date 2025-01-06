namespace ASD.Onboard.Application.Common.Extensions;

public static class CustomGuardClauseExtensions
{
    public static void EmailNotConfirmed(this IGuardClause guardClause, bool emailConfirmed, string parameterName)
    {
        if (!emailConfirmed)
        {
            throw new EmailNotConfirmedException("Please confirm your email before logging in. Check your inbox for the confirmation link.");
        }
    }

    public static void InvalidCredentials(this IGuardClause guardClause, bool condition, string parameterName)
    {
        if (condition)
        {
            throw new UnauthorizedAccessException("Invalid credentials. Please check your email and password.");
        }
    }
}

public class EmailNotConfirmedException : UnauthorizedAccessException
{
    public EmailNotConfirmedException(string message) : base(message) { }
}

