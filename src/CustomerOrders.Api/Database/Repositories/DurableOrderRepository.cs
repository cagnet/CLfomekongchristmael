using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrders.Api.Database.Repositories;

public class DurableOrderRepository(CustomerOrdersDbContext dbContext): IOrderRepository
    
{
    public async Task<IReadOnlyCollection<Order>> GetAll()
    {
        return (await dbContext.Orders.ToListAsync()).AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Order>> GetByCustomerId(int customerId)
    {
        return await dbContext.Orders.Where(o => o.CustomerId == customerId)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<Order?> GetById(int id)
    {
        return dbContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> Add(int customerId, decimal amount, DateTime createdAt)
    {
        var result = await dbContext.Orders.AddAsync(new Order()
        {
            CustomerId = customerId,
            Amount = amount,
            CreatedAt = createdAt
        });
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public Task Update(Order order)
    {
        return dbContext.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var existingOrder = await dbContext.Orders.FirstOrDefaultAsync(c => c.Id == id);
        if (existingOrder is null) return false;
        dbContext.Orders.Remove(existingOrder);
        return true;
    }

    public Task<bool> ExistsForCustomer(int customerId)
    {
        return dbContext.Orders.AnyAsync(o => o.CustomerId == customerId);
    }
}