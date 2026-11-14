using MarketPlace.Api.DTOs.Orders;
using MarketPlace.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Api.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController(IOrderService orders) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<OrderResponse>> Create([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await orders.CreateAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, order);
    }
}
