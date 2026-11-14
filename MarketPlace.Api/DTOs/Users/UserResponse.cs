using MarketPlace.Api.Entities;

namespace MarketPlace.Api.DTOs.Users;

public sealed record UserResponse(int Id, string FullName, string Email, UserRole Role);
