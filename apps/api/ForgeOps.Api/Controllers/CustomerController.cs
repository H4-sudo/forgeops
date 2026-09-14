using ForgeOps.Api.Data;
using ForgeOps.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ForgeOps.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Customer>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetCustomers(ForgeOpsDbContext dbContext)
    {
        var customers = dbContext.Customers.ToList();

        return Ok(customers);
    }
}
