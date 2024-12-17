using ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Commands;
using ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Models;
using ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Queries;
using ASD.Onboard.Application.Features.ApplicantProfiles.Applicants.Commands;
using ASD.Onboard.Application.Features.ApplicantProfiles.Applicants.Models;
using ASD.Onboard.Application.Features.ApplicantProfiles.Applicants.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ASD.Onboard.Web.Controllers;

public class ApplicantController : BaseController
{
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApplicantModel>> Get(Guid id)
    {
        var result = await Mediator.Send(new GetApplicantQuery(id));
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Create([FromBody] CreateApplicantCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Update(UpdateApplicantCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    #region Educations
    [HttpGet("{applicantId:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ApplicantEducationModel>>> GetEducations(Guid appliacantId)
    {
        var result = await Mediator.Send(new GetApplicantEducationQuery(appliacantId));
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpsertEducations(UpsertApplicantEducationCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    #endregion

}
