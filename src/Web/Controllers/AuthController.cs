using ASD.Onboard.Application.Features.Identity.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASD.Onboard.Web.Controllers;

public class AuthController : BaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserRegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Succeeded)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromBody] LoginRequestCommand command, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(command, cancellationToken);
        if (response == null)
            return Unauthorized("Invalid username or password.");
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Succeeded)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        if (!result.Succeeded)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var changePasswordCommand = new ChangePasswordCommand(command.OldPassword, command.NewPassword);
        var result = await Mediator.Send(changePasswordCommand, cancellationToken);
        if (!result.Succeeded)
            return BadRequest(result);
        return Ok(result);
    }
}


