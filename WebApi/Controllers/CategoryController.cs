using Flora.Application.Categories.Commands.CreateCategory;
using Flora.Application.Categories.Commands.DeleteCategory;
using Flora.Application.Categories.Commands.UpdateCategory;
using Flora.Application.Categories.Queries.GetCategories;
using Flora.Application.Categories.Queries.GetCategory;
using Flora.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flora.WebApi.Controllers;

[ApiVersionNeutral]
public class CategoryController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CategoryDto>> Get([FromQuery] GetCategoryQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet]
    public async Task<ActionResult<Collection<CategoryBriefDto>>> GetAll([FromQuery] GetCategoriesQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Roles = "")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateCategoryCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut]
    [Authorize(Roles = "")]
    public async Task<ActionResult> Update([FromBody] UpdateCategoryCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete]
    [Authorize(Roles = "")]
    public async Task<ActionResult> Delete([FromBody] DeleteCategoryCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
}