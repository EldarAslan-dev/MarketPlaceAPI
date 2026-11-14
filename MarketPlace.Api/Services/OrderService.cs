using MarketPlace.Api.Data;
using MarketPlace.Api.DTOs.Orders;
using MarketPlace.Api.Entities;
using MarketPlace.Api.Exceptions;
using MarketPlace.Api.Repositories.Interfaces;
using MarketPlace.Api.Services.Interfaces;

namespace MarketPlace.Api.Services;

public sealed class OrderService(
    IUserRepository users,
    IProductRepository products,
    IOrderRepository orders,
    IUnitOfWork unitOfWork) : IOrderService
{
    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var buyer = await users.GetByIdAsync(request.BuyerId, cancellationToken)
            ?? throw new ApiException(404, "buyer_not_found", "Buyer was not found.");

        if (buyer.Role != UserRole.Buyer)
            throw new ApiException(403, "buyer_required", "Only buyers can place orders.");

        var product = await products.GetByIdAsync(request.ProductId, cancellationToken)
            ?? throw new ApiException(404, "product_not_found", "Product was not found.");

        var order = new Order
        {
            BuyerId = buyer.Id,
            ProductId = product.Id,
            Quantity = request.Quantity,
            UnitPrice = product.Price,
            TotalPrice = product.Price * request.Quantity
        };

        await unitOfWork.ExecuteInTransactionAsync(async token =>
        {
            if (!await products.TryDecrementStockAsync(product.Id, request.Quantity, token))
                throw new ApiException(409, "insufficient_stock", "Not enough stock for this order.");

            await orders.AddAsync(order, token);
        }, cancellationToken);

        return Map(order, product.Name);
    }

    public async Task<IReadOnlyList<OrderResponse>> GetByBuyerIdAsync(int buyerId, CancellationToken cancellationToken = default)
    {
        var buyer = await users.GetByIdAsync(buyerId, cancellationToken)
            ?? throw new ApiException(404, "buyer_not_found", "Buyer was not found.");

        if (buyer.Role != UserRole.Buyer)
            throw new ApiException(403, "buyer_required", "User is not a buyer.");

        return (await orders.GetByBuyerIdAsync(buyerId, cancellationToken))
            .Select(order => Map(order, order.Product.Name)).ToList();
    }

    private static OrderResponse Map(Order order, string productName) =>
        new(order.Id, order.BuyerId, order.ProductId, productName,
            order.Quantity, order.UnitPrice, order.TotalPrice, order.CreatedAt);
}
