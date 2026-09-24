using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrders.Api.Database.Repositories;

public class DurableCustomerRepository(CustomerOrdersDbContext dbContext):ICustomerRepository
{
    public async Task<IReadOnlyCollection<Customer>> GetAll()
    {
        return await dbContext.Customers
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<Customer?> GetById(int id)
    {
        return dbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer> Add(string name, string firstName, string email, string address, bool isActive)
    {
        var result = await dbContext.Customers.AddAsync(new Customer()
        {
            Name = name,
            FirstName = firstName,
            Email = email,
            Address = address,
            IsActive = isActive
        });
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public Task Update(Customer customer)
    {
        return dbContext.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var existingCustomer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
        if (existingCustomer is null) return false;
        dbContext.Customers.Remove(existingCustomer);
        return true;
    }
}