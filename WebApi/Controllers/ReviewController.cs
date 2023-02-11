using Flora.Application.Common.Models;
using Flora.Application.Reviews.Commands.CreateReview;
using Flora.Application.Reviews.Commands.DeleteReview;
using Flora.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flora.WebApi.Controllers;

[ApiVersionNeutral]
public class ReviewController : ApiControllerBase
{
    [HttpPost]
    [Authorize(Roles = $"{nameof(Role.Administrator)},{nameof(Role.Buyer)}")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateReviewCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpDelete]
    [Authorize(Roles = $"{nameof(Role.Administrator)},{nameof(Role.Buyer)}")]
    public async Task<ActionResult> Delete([FromBody] DeleteReviewCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
}