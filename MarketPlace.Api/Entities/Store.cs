namespace MarketPlace.Api.Entities;

public sealed class Store
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
