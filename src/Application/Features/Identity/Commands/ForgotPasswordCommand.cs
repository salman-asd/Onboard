using ASD.Onboard.Application.Common.Interfaces.Identity;

namespace ASD.Onboard.Application.Features.Identity.Commands;

public record ForgotPasswordCommand(string Email): IRequest<Result>;

internal sealed class ForgotPasswordCommandHandler(IAuthService authService) : IRequestHandler<ForgotPasswordCommand, Result>
{
    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        return await authService.ForgotPasswordAsync(request.Email);
    }
}
