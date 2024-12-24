
namespace ASD.Onboard.Application.Features.Identity.Commands;

public record LoginRequestCommand(string Username, string Password): IRequest;

internal sealed class LoginRequestCommandHandler : IRequestHandler<LoginRequestCommand>
{
    public async Task Handle(LoginRequestCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
