using System.ComponentModel.DataAnnotations;

namespace ForgeOps.Api.Models;

public sealed class CustomerModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "First name is required.")]
    public required string FirstName { get; set; }
    [Required(ErrorMessage = "Last name is required.")]
    public required string LastName { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public required string Email { get; set; }
    [Required(ErrorMessage = "Phone number is required.")]
    public required string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Address is required.")]
    public required string Address { get; set; }
    [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
