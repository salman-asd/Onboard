namespace ASD.Onboard.Application.Features.Identity.Commands;

public record UserRegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNo,
    string Password): IRequest<Result>;

internal sealed class UserRegisterCommandHandler(IIdentityService identityService) 
    : IRequestHandler<UserRegisterCommand, Result>
{
    public async Task<Result> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
    {
        return await identityService.CreateUserAsync(request);
    }
}
