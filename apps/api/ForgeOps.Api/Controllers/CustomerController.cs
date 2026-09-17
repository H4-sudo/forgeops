using ForgeOps.Api.Models;
using ForgeOps.Api.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForgeOps.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController(ILogger<CustomerController> _logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(Shared.BaseForgeOpsPagination<Models.CustomerModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetCustomers(Data.ForgeOpsDbContext dbContext, int page = 1, int pageSize = 10, string searchTerm = "")
    {
        if (page <= 0 || pageSize <= 0) return BadRequest("Page and PageSize must be greater than 0.");
        if (pageSize > 100) return BadRequest("PageSize cannot be greater than 100.");

        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            var customers = SearchCustomersByTermPaginated(dbContext, searchTerm, page, pageSize);
            return Ok(customers);
        }

        var allCustomers = OrderDataByLastNameAndFirstNamePaginated(dbContext, page, pageSize);
        return Ok(allCustomers);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Dtos.CustomerResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Shared.BaseForgeOpsError), StatusCodes.Status500InternalServerError)]
    public IActionResult CreateCustomer(
    Data.ForgeOpsDbContext dbContext,
    [FromBody] Dtos.CustomerDto customer)
    {
        var newCustomer = new Models.CustomerModel
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Address = customer.Address,
            Notes = customer.Notes
        };

        try
        {
            dbContext.Customers.Add(newCustomer);
            dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            return ErrorResponse.HandleServerError(_logger, e, "An error occurred while saving the customer");
        }

        return CreatedAtAction(nameof(GetCustomerById), new { id = newCustomer.Id }, newCustomer);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Dtos.CustomerResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetCustomerById(Data.ForgeOpsDbContext dbContext, int id)
    {
        var customer = dbContext.Customers.FirstOrDefault(c => c.Id == id);

        if (customer == null)
        {
            return NotFound();
        }

        var customerResponse = new Dtos.CustomerResponseDto(
            Id: customer.Id,
            FirstName: customer.FirstName,
            LastName: customer.LastName,
            Email: customer.Email,
            PhoneNumber: customer.PhoneNumber,
            Address: customer.Address,
            Notes: customer.Notes,
            CreatedAt: customer.CreatedAt,
            UpdatedAt: customer.UpdatedAt
        );

        return Ok(customerResponse);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Dtos.CustomerResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Shared.BaseForgeOpsError), StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateCustomer(Data.ForgeOpsDbContext dbContext, int id, [FromBody] Dtos.CustomerDto customer)
    {
        var existingCustomer = dbContext.Customers.FirstOrDefault(c => c.Id == id);

        if (existingCustomer == null)
        {
            return NotFound();
        }

        existingCustomer.FirstName = customer.FirstName;
        existingCustomer.LastName = customer.LastName;
        existingCustomer.Email = customer.Email;
        existingCustomer.PhoneNumber = customer.PhoneNumber;
        existingCustomer.Address = customer.Address;
        existingCustomer.Notes = customer.Notes;
        existingCustomer.UpdatedAt = DateTime.UtcNow;

        try
        {
            dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            return ErrorResponse.HandleServerError(_logger, e, "An error occurred while updating the customer");
        }

        var customerResponse = new Dtos.CustomerResponseDto(
            Id: existingCustomer.Id,
            FirstName: existingCustomer.FirstName,
            LastName: existingCustomer.LastName,
            Email: existingCustomer.Email,
            PhoneNumber: existingCustomer.PhoneNumber,
            Address: existingCustomer.Address,
            Notes: existingCustomer.Notes,
            CreatedAt: existingCustomer.CreatedAt,
            UpdatedAt: existingCustomer.UpdatedAt
        );

        return Ok(customerResponse);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Shared.BaseForgeOpsError), StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteCustomer(Data.ForgeOpsDbContext dbContext, int id)
    {
        var existingCustomer = dbContext.Customers.FirstOrDefault(c => c.Id == id);

        if (existingCustomer == null)
        {
            return NotFound();
        }

        try
        {
            dbContext.Customers.Remove(existingCustomer);
            dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            return ErrorResponse.HandleServerError(_logger, e, "An error occurred while deleting the customer");
        }

        return NoContent();
    }

    private Shared.BaseForgeOpsPagination<Dtos.CustomerResponseDto> OrderDataByLastNameAndFirstNamePaginated(Data.ForgeOpsDbContext dbContext, int page, int pageSize)
    {
        var orderedCustomers = dbContext.Customers
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        var paginationModel = new Shared.BaseForgeOpsPagination<Dtos.CustomerResponseDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = dbContext.Customers.Count(),
            Items = orderedCustomers.Select(c => new Dtos.CustomerResponseDto(
                Id: c.Id,
                FirstName: c.FirstName,
                LastName: c.LastName,
                Email: c.Email,
                PhoneNumber: c.PhoneNumber,
                Address: c.Address,
                Notes: c.Notes,
                CreatedAt: c.CreatedAt,
                UpdatedAt: c.UpdatedAt
            ))
        };
        return paginationModel;
    }

    private Shared.BaseForgeOpsPagination<Dtos.CustomerResponseDto> SearchCustomersByTermPaginated(Data.ForgeOpsDbContext dbContext, string searchTerm, int page, int pageSize)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        var filteredCustomers = dbContext.Customers
            .Where(c => c.FirstName.ToLower().Contains(lowerSearchTerm) ||
                        c.LastName.ToLower().Contains(lowerSearchTerm) ||
                        c.Email.ToLower().Contains(lowerSearchTerm) ||
                        c.PhoneNumber.ToLower().Contains(lowerSearchTerm));
        var totalCount = filteredCustomers.Count();

        var orderedFilteredCustomers = filteredCustomers
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var paginationModel = new Shared.BaseForgeOpsPagination<Dtos.CustomerResponseDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = orderedFilteredCustomers.Select(c => new Dtos.CustomerResponseDto(
                Id: c.Id,
                FirstName: c.FirstName,
                LastName: c.LastName,
                Email: c.Email,
                PhoneNumber: c.PhoneNumber,
                Address: c.Address,
                Notes: c.Notes,
                CreatedAt: c.CreatedAt,
                UpdatedAt: c.UpdatedAt
            ))
        };
        return paginationModel;
    }
}
