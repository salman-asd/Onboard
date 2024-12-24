
namespace ASD.Onboard.Application.Features.Identity.Commands;

public record ForgotPasswordCommand(string Email): IRequest;

internal sealed class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
{
    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
