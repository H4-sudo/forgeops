namespace ForgeOps.Api.Dtos;

public sealed record CustomerDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Address { get; init; }
    public string Notes { get; init; } = string.Empty;
}
