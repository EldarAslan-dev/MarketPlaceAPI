namespace MarketPlace.Api.Entities;

public sealed class User
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public UserRole Role { get; set; }
    public ICollection<Store> Stores { get; set; } = new List<Store>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
