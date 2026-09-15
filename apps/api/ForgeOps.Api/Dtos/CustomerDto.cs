using System.ComponentModel.DataAnnotations;

namespace ForgeOps.Api.Dtos;

public sealed record CustomerDto
{
    [Required]
    public required string FirstName { get; init; }
    [Required]
    public required string LastName { get; init; }
    [EmailAddress]
    public required string Email { get; init; }
    [Required]
    public required string PhoneNumber { get; init; }
    [Required]
    public required string Address { get; init; }
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string Notes { get; init; } = string.Empty;
}
