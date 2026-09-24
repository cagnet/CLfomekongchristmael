using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Repositories;
using CustomerOrders.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrders.Infrastructure.Persistence.Repositories;

public class DurableOrderRepository(CustomerOrdersDbContext dbContext): IOrderRepository
    
{
    public async Task<IReadOnlyCollection<Order>> GetAll(CancellationToken cancellationToken)
    {
        return (await dbContext.Orders.ToListAsync(cancellationToken)).AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Order>> GetByCustomerId(int customerId, CancellationToken cancellationToken)
    {
        return await dbContext.Orders.Where(o => o.CustomerId == customerId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<Order?> GetById(int id, CancellationToken cancellationToken)
    {
        return dbContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Order> Add(int customerId, decimal amount, DateTime createdAt, CancellationToken cancellationToken)
    {
        var result = await dbContext.Orders.AddAsync(new Order()
        {
            CustomerId = customerId,
            Amount = amount,
            CreatedAt = createdAt
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public Task Update(Order order, CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> Delete(int id, CancellationToken cancellationToken)
    {
        var existingOrder = await dbContext.Orders.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (existingOrder is null) return false;
        dbContext.Orders.Remove(existingOrder);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public Task<bool> ExistsForCustomer(int customerId, CancellationToken cancellationToken)
    {
        return dbContext.Orders.AnyAsync(o => o.CustomerId == customerId, cancellationToken);
    }
}