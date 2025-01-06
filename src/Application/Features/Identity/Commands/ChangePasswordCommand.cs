using ASD.Onboard.Application.Common.Security;

namespace ASD.Onboard.Application.Features.Identity.Commands;

[Authorize]
public record ChangePasswordCommand(
    string OldPassword, 
    string NewPassword): IRequest<Result>;

internal sealed class ChangePasswordCommandHandler(
    IAuthService authService, 
    IUser user) : IRequestHandler<ChangePasswordCommand, Result>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        return await authService.ChangePasswordAsync(user.Id, request.OldPassword, request.NewPassword);
    }
}
