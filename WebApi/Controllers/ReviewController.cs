using Flora.Application.Reviews.Commands.CreateReview;
using Microsoft.AspNetCore.Mvc;

namespace Flora.WebApi.Controllers;

[ApiVersionNeutral]
public class ReviewController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateReviewCommand command)
    {
        return await Mediator.Send(command);
    }
}