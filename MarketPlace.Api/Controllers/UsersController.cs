using MarketPlace.Api.DTOs.Orders;
using MarketPlace.Api.DTOs.Users;
using MarketPlace.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController(IUserService users, IOrderService orders) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<UserResponse>> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await users.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetById(int id, CancellationToken cancellationToken) =>
        Ok(await users.GetByIdAsync(id, cancellationToken));

    [HttpGet("{id:int}/orders")]
    public async Task<ActionResult<IReadOnlyList<OrderResponse>>> GetOrders(int id, CancellationToken cancellationToken) =>
        Ok(await orders.GetByBuyerIdAsync(id, cancellationToken));
}
