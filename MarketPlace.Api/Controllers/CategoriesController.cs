using MarketPlace.Api.DTOs.Categories;
using MarketPlace.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Api.Controllers;

[ApiController]
[Route("api/categories")]
public sealed class CategoriesController(ICategoryService categories) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<CategoryResponse>> Create([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await categories.CreateAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, category);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CategoryResponse>>> List(CancellationToken cancellationToken) =>
        Ok(await categories.ListAsync(cancellationToken));
}
