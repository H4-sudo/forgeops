namespace ForgeOps.Api.Dtos;

public sealed record CustomerResponseDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Address,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
