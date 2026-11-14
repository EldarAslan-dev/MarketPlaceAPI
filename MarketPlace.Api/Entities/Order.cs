namespace MarketPlace.Api.Entities;

public sealed class Order
{
    public int Id { get; set; }
    public int BuyerId { get; set; }
    public User Buyer { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
