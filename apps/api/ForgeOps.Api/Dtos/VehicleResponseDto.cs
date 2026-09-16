namespace ForgeOps.Api.Dtos;

public sealed record VehicleResponseDto(
    int Id,
    int CurrentOwnerId,
    string Make,
    string Model,
    string Year,
    string VIN,
    decimal Odometer,
    string Color,
    string EngineType,
    string TransmissionType,
    string LicensePlate,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt
);