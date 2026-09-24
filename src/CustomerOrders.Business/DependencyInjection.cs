using CustomerOrders.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerOrders.Business;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<CustomerService>();
        serviceCollection.AddScoped<OrderService>();
    }
    
}