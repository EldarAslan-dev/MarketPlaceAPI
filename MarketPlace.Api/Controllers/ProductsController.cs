using MarketPlace.Api.DTOs.Common;
using MarketPlace.Api.DTOs.Products;
using MarketPlace.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(IProductService products) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<ProductResponse>> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await products.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponse>> GetById(int id, CancellationToken cancellationToken) =>
        Ok(await products.GetByIdAsync(id, cancellationToken));

    [HttpGet]
    public async Task<ActionResult<PagedResponse<ProductResponse>>> Search([FromQuery] ProductSearchQuery query, CancellationToken cancellationToken) =>
        Ok(await products.SearchAsync(query, cancellationToken));
}
