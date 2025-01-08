namespace ASD.Onboard.Application.Common.Exceptions;

public class UserNotFoundException: UnauthorizedAccessException
{
    public UserNotFoundException(string message) : base(message) { }
}
