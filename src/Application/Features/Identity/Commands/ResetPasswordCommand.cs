namespace ASD.Onboard.Application.Features.Identity.Commands;

public record ResetPasswordCommand(
    string Email, 
    string Token,  
    string NewPassword): IRequest<Result>;

internal sealed class ResetPasswordCommandHandler(IAuthService authService) 
    : IRequestHandler<ResetPasswordCommand, Result>
{
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        return await authService.ResetPasswordAsync(request.Email, request.Token, request.NewPassword);
    }
}
