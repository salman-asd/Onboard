using ASD.Onboard.Application.Common.Exceptions;

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

    public static void InvalidUserCredential(this IGuardClause guardClause, object user)
    {
        if (user is null)
        {
            throw new InvalidUserCredentialException("User not found or invalid credentials.");
        }
    }

    public static void InvalidCredential(this IGuardClause guardClause, bool isValid)
    {
        if (!isValid)
        {
            throw new InvalidUserCredentialException("User not found or invalid credentials.");
        }
    }
}

