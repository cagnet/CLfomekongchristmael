using System.Reflection;
using CustomerOrders.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrders.Api.Database;

public class CustomerOrdersDbContext(DbContextOptions<CustomerOrdersDbContext> options): DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerOrdersDbContext).Assembly);

    }
}