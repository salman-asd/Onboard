using ASD.Onboard.Application.Features.PositionPosts.Commands;
using ASD.Onboard.Application.Features.PositionPosts.Models;
using ASD.Onboard.Application.Features.PositionPosts.Queries;
using ASD.Onboard.Domain.Entities.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace ASD.Onboard.Web.Controllers;

public class PositionPostController : BaseController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<JobCardModel>> GetJobCards()
    {
        var result = await Mediator.Send(new GetPositionPostsForJobCardsQuery());
        return Ok(result);
    }

    [HttpGet("{positionPostId:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<JobCardModel>> GetJobDetails([FromRoute] Guid positionPostId)
    {
        var result = await Mediator.Send(new GetPositionPostDetailQuery(positionPostId));
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ApplyJob([FromBody] CreateJobApplicationCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    //[HttpPut]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //public async Task<ActionResult> Update(UpdatePositionPostCommand command)
    //{
    //    await Mediator.Send(command);
    //    return Ok();
    //}



}
