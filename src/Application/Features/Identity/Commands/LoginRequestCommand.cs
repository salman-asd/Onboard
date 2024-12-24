using ASD.Onboard.Application.Features.Identity.Models;

namespace ASD.Onboard.Application.Features.Identity.Commands;

public record LoginRequestCommand(string Username, string Password): IRequest<AuthResponse>;

internal sealed class LoginRequestCommandHandler(IAuthService authService) 
    : IRequestHandler<LoginRequestCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(LoginRequestCommand request, CancellationToken cancellationToken)
    {
        return await authService.LoginAsync(request.Username, request.Password);
    }
}
