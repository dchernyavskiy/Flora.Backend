using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Flora.WebApi.Controllers;

[ApiController]
[Route("api/v{apiVersion}/[controller]/[action]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
