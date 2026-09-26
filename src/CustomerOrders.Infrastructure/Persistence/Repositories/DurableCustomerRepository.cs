using CustomerOrders.Business.Contracts;
using CustomerOrders.Domain.Exceptions;
using CustomerOrders.Domain.Models;
using CustomerOrders.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrders.Infrastructure.Persistence.Repositories;

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
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Customer> Add(string name, string firstName, 
        string email, string address, bool isActive, CancellationToken cancellationToken)
    {
        var existingCustomer =
            await dbContext.Customers.FirstOrDefaultAsync(
                c => c.Email.ToLower() == email.ToLower(), cancellationToken);
        if (existingCustomer is not null)
            throw new BusinessConflictException(BusinessRuleCodes.Conflicts.EmailAlreadyTaken,
                "The email provided is already taken.");
        var result = await dbContext.Customers.AddAsync(new Customer()
        {
            Name = name,
            FirstName = firstName,
            Email = email,
            Address = address,
            IsActive = isActive
        }, cancellationToken);
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