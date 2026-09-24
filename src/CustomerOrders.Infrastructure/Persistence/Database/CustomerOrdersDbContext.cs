using CustomerOrders.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrders.Infrastructure.Persistence.Database;

public class CustomerOrdersDbContext(DbContextOptions<CustomerOrdersDbContext> options): DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerOrdersDbContext).Assembly);

    }
}