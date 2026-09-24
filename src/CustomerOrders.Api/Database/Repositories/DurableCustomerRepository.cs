using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrders.Api.Database.Repositories;

public class DurableCustomerRepository(CustomerOrdersDbContext dbContext):ICustomerRepository
{
    public async Task<IReadOnlyCollection<Customer>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.Customers
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<Customer?> GetById(int id, CancellationToken cancellationToken)
    {
        return dbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Customer> Add(string name, string firstName, 
        string email, string address, bool isActive, CancellationToken cancellationToken)
    {
        var result = await dbContext.Customers.AddAsync(new Customer()
        {
            Name = name,
            FirstName = firstName,
            Email = email,
            Address = address,
            IsActive = isActive
        });
        await dbContext.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public Task Update(Customer customer, CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> Delete(int id, CancellationToken cancellationToken)
    {
        var existingCustomer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
        if (existingCustomer is null) return false;
        dbContext.Customers.Remove(existingCustomer);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}