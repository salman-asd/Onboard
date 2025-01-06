namespace ASD.Onboard.Application.Features.Identity.Commands;

public record ConfirmEmailCommand(
    string Email, 
    string Token): IRequest<Result>;

internal sealed class ConfirmEmailCommandHandler(
    IAuthService authService) : IRequestHandler<ConfirmEmailCommand, Result>
{
    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        return await authService.ConfirmEmailAsync(request.Email, request.Token, cancellationToken);
    }
}
