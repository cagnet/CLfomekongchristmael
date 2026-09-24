using CustomerOrders.Business.Contracts;
using CustomerOrders.Infrastructure.Persistence.Database;
using CustomerOrders.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerOrders.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, String connectionString)
    {
        services.AddDbContext<CustomerOrdersDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        services.AddScoped<ICustomerRepository, DurableCustomerRepository>();
        services.AddScoped<IOrderRepository, DurableOrderRepository>();
        
    }
}