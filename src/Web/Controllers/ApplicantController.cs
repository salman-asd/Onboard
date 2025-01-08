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
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApplicantModel>> Get()
    {
        var result = await Mediator.Send(new GetApplicantQuery());
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Create([FromBody] CreateApplicantCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Update(UpdateApplicantCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    #region Educations
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ApplicantEducationModel>>> GetEducations()
    {
        var result = await Mediator.Send(new GetApplicantEducationQuery());
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpsertEducations([FromBody] List<ApplicantEducationModel> applicantEducations)
    {
        await Mediator.Send(new UpsertApplicantEducationCommand(applicantEducations));
        return Ok();
    }
    #endregion

}
