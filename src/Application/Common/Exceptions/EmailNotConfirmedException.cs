namespace ASD.Onboard.Application.Common.Exceptions;

public class EmailNotConfirmedException : UnauthorizedAccessException
{
    public EmailNotConfirmedException(string message) : base(message) { }
}

