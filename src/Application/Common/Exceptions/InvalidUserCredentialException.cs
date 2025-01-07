namespace ASD.Onboard.Application.Common.Exceptions;

public class InvalidUserCredentialException : UnauthorizedAccessException
{
    public InvalidUserCredentialException(string message) : base(message) { }
}

