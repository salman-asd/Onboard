
namespace ASD.Onboard.Application.Features.Identity.Commands;

public record ChangePasswordCommand(string OldPassword, string NewPassword): IRequest;

internal sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
{
    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
