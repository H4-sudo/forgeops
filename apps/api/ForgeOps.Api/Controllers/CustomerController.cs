using Microsoft.AspNetCore.Mvc;

namespace ForgeOps.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Models.CustomerModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetCustomers(Data.ForgeOpsDbContext dbContext)
    {
        var customers = dbContext.Customers.ToList();

        return Ok(customers);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Models.CustomerModel), StatusCodes.Status201Created)]
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

        dbContext.Customers.Add(newCustomer);
        dbContext.SaveChanges();

        return Created("", newCustomer);
    }
}
