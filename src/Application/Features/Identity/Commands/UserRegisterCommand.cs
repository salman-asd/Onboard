
namespace ASD.Onboard.Application.Features.Identity.Commands;

public record UserRegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNo,
    string Password): IRequest
{
}

internal sealed class UserRegisterCommandHandler : IRequestHandler<UserRegisterCommand>
{
    public async Task Handle(UserRegisterCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
