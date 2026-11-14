using MarketPlace.Api.DTOs.Products;
using MarketPlace.Api.DTOs.Stores;
using MarketPlace.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Api.Controllers;

[ApiController]
[Route("api/stores")]
public sealed class StoresController(IStoreService stores, IProductService products) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(StoreResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<StoreResponse>> Create([FromBody] CreateStoreRequest request, CancellationToken cancellationToken)
    {
        var store = await stores.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = store.Id }, store);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<StoreResponse>>> List(CancellationToken cancellationToken) =>
        Ok(await stores.ListAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StoreResponse>> GetById(int id, CancellationToken cancellationToken) =>
        Ok(await stores.GetByIdAsync(id, cancellationToken));

    [HttpGet("{id:int}/products")]
    public async Task<ActionResult<IReadOnlyList<ProductResponse>>> GetProducts(int id, CancellationToken cancellationToken) =>
        Ok(await products.GetByStoreIdAsync(id, cancellationToken));
}
