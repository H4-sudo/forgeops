using ForgeOps.Api.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForgeOps.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleController(ILogger<VehicleController> logger) : ControllerBase
{
    [HttpPost("customer/{customerId:int}/vehicles")]
    [ProducesResponseType(typeof(Dtos.VehicleResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateVehicle(Data.ForgeOpsDbContext dbContext, [FromRoute] int customerId, Dtos.CreateVehicleDto vehicleDto)
    {
        var newCustomerVehicle = new Models.VehicleModel
        {
            CurrentOwnerId = customerId,
            Make = vehicleDto.Make,
            Model = vehicleDto.Model,
            Year = vehicleDto.Year,
            VIN = vehicleDto.VIN,
            Odometer = vehicleDto.Odometer,
            Color = vehicleDto.Color,
            EngineType = vehicleDto.EngineType,
            TransmissionType = vehicleDto.TransmissionType,
            LicensePlate = vehicleDto.LicensePlate,
            Notes = vehicleDto.Notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        try
        {
            dbContext.Vehicles.Add(newCustomerVehicle);
            dbContext.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            return ErrorResponse.HandleServerError(logger, ex, "An error occurred while saving the vehicle");
        }

        return CreatedAtAction(nameof(GetVehicle), new { id = newCustomerVehicle.Id }, new Dtos.VehicleResponseDto(
            newCustomerVehicle.Id,
            newCustomerVehicle.CurrentOwnerId,
            newCustomerVehicle.Make,
            newCustomerVehicle.Model,
            newCustomerVehicle.Year,
            newCustomerVehicle.VIN,
            newCustomerVehicle.Odometer,
            newCustomerVehicle.Color,
            newCustomerVehicle.EngineType,
            newCustomerVehicle.TransmissionType,
            newCustomerVehicle.LicensePlate,
            newCustomerVehicle.Notes,
            newCustomerVehicle.CreatedAt,
            newCustomerVehicle.UpdatedAt
        ));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Dtos.VehicleResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetVehicle(Data.ForgeOpsDbContext dbContext, [FromRoute] int id)
    {
        var vehicle = dbContext.Vehicles.Find(id);
        if (vehicle == null)
        {
            return NotFound();
        }

        return Ok(new Dtos.VehicleResponseDto(
            vehicle.Id,
            vehicle.CurrentOwnerId,
            vehicle.Make,
            vehicle.Model,
            vehicle.Year,
            vehicle.VIN,
            vehicle.Odometer,
            vehicle.Color,
            vehicle.EngineType,
            vehicle.TransmissionType,
            vehicle.LicensePlate,
            vehicle.Notes,
            vehicle.CreatedAt,
            vehicle.UpdatedAt
        ));
    }

    [HttpGet("customer/{customerId:int}/vehicles")]
    [ProducesResponseType(typeof(IEnumerable<Dtos.VehicleResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetVehiclesByCustomer(Data.ForgeOpsDbContext dbContext, [FromRoute] int customerId)
    {
        if (!dbContext.Customers.Any(c => c.Id == customerId))
            return NotFound();

        var vehicles = dbContext.Vehicles.Where(v => v.CurrentOwnerId == customerId);

        return Ok(vehicles.Select(v => new Dtos.VehicleResponseDto(
            v.Id,
            v.CurrentOwnerId,
            v.Make,
            v.Model,
            v.Year,
            v.VIN,
            v.Odometer,
            v.Color,
            v.EngineType,
            v.TransmissionType,
            v.LicensePlate,
            v.Notes,
            v.CreatedAt,
            v.UpdatedAt
        )));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Dtos.VehicleResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateVehicle(Data.ForgeOpsDbContext dbContext, [FromRoute] int id, Dtos.CreateVehicleDto vehicleDto)
    {
        var vehicle = dbContext.Vehicles.Find(id);
        if (vehicle == null)
        {
            return NotFound();
        }

        vehicle.Make = vehicleDto.Make;
        vehicle.Model = vehicleDto.Model;
        vehicle.Year = vehicleDto.Year;
        vehicle.VIN = vehicleDto.VIN;
        vehicle.Odometer = vehicleDto.Odometer;
        vehicle.Color = vehicleDto.Color;
        vehicle.EngineType = vehicleDto.EngineType;
        vehicle.TransmissionType = vehicleDto.TransmissionType;
        vehicle.LicensePlate = vehicleDto.LicensePlate;
        vehicle.Notes = vehicleDto.Notes;
        vehicle.UpdatedAt = DateTime.UtcNow;

        try
        {
            dbContext.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            return ErrorResponse.HandleServerError(logger, ex, "An error occurred while updating the vehicle");
        }

        return Ok(new Dtos.VehicleResponseDto(
            vehicle.Id,
            vehicle.CurrentOwnerId,
            vehicle.Make,
            vehicle.Model,
            vehicle.Year,
            vehicle.VIN,
            vehicle.Odometer,
            vehicle.Color,
            vehicle.EngineType,
            vehicle.TransmissionType,
            vehicle.LicensePlate,
            vehicle.Notes,
            vehicle.CreatedAt,
            vehicle.UpdatedAt
        ));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteVehicle(Data.ForgeOpsDbContext dbContext, [FromRoute] int id)
    {
        var vehicle = dbContext.Vehicles.Find(id);
        if (vehicle == null)
        {
            return NotFound();
        }

        try
        {
            dbContext.Vehicles.Remove(vehicle);
            dbContext.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            return ErrorResponse.HandleServerError(logger, ex, "An error occurred while deleting the vehicle");
        }
        return NoContent();
    }
}
