
namespace ASD.Onboard.Application.Features.Identity.Commands;

public record ResetPasswordCommand(Guid UserId,  string Password): IRequest;

internal sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
