using System.ComponentModel.DataAnnotations;
using MarketPlace.Api.Entities;

namespace MarketPlace.Api.DTOs.Users;

public sealed class CreateUserRequest
{
    [Required, StringLength(120, MinimumLength = 2)]
    public required string FullName { get; init; }

    [Required, EmailAddress, StringLength(256)]
    public required string Email { get; init; }

    public UserRole Role { get; init; }
}
