using ForgeOps.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ForgeOps.Api.Data;

public class ForgeOpsDbContext(DbContextOptions<ForgeOpsDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
}

