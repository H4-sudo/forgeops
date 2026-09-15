using System.ComponentModel.DataAnnotations;

namespace ForgeOps.Api.Models;

public sealed class VehicleModel
{
    public int Id { get; set; }
    public int CurrentOwnerId { get; set; }
    public CustomerModel CurrentOwner { get; set; } = null!;
    [Required(ErrorMessage = "Make is required.")]
    public required string Make { get; set; }
    [Required(ErrorMessage = "Model is required.")]
    public required string Model { get; set; }
    [Required(ErrorMessage = "Year is required.")]
    public required string Year { get; set; }
    [Required(ErrorMessage = "VIN is required.")]
    public required string VIN { get; set; }
    [Required(ErrorMessage = "Odometer is required.")]
    public required decimal Odometer { get; set; }
    [Required(ErrorMessage = "Color is required.")]
    public required string Color { get; set; }
    [Required(ErrorMessage = "Engine type is required.")]
    public required string EngineType { get; set; }
    [Required(ErrorMessage = "Transmission type is required.")]
    public required string TransmissionType { get; set; }
    [Required(ErrorMessage = "License plate is required.")]
    public required string LicensePlate { get; set; }
    [MaxLength(750, ErrorMessage = "Notes cannot exceed 750 characters.")]
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}
