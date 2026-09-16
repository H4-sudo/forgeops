using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ForgeOps.Api.Dtos;

public sealed record CreateVehicleDto
{
    [Required(ErrorMessage = "Make is required.")]
    public required string Make { get; init; }
    [Required(ErrorMessage = "Model is required.")]
    public required string Model { get; init; }
    [Required(ErrorMessage = "Year is required.")]
    public required string Year { get; init; }
    [Required(ErrorMessage = "VIN is required.")]
    public required string VIN { get; init; }
    [Required(ErrorMessage = "Odometer is required.")]
    public required decimal Odometer { get; init; }
    [Required(ErrorMessage = "Color is required.")]
    public required string Color { get; init; }
    [Required(ErrorMessage = "Engine type is required.")]
    public required string EngineType { get; init; }
    [Required(ErrorMessage = "Transmission type is required.")]
    public required string TransmissionType { get; init; }
    [Required(ErrorMessage = "License plate is required.")]
    public required string LicensePlate { get; init; }
    [MaxLength(750, ErrorMessage = "Notes cannot exceed 750 characters.")]
    public string? Notes { get; init; }
}
