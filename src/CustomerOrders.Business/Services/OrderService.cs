using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Exceptions;
using CustomerOrders.Business.Repositories;

namespace CustomerOrders.Business.Services;

public sealed class OrderService(ICustomerRepository customers, IOrderRepository orders)
{
    public Task<IReadOnlyCollection<Order>> ReadAll() => orders.GetAll();
    public Task<Order?> ReadOne(int id) => orders.GetById(id);

    public async Task<Order> Create(int customerId, decimal amount)
    {
        var customer = await customers.GetById(customerId);
        if (customer is null) 
            throw new CustomerNotFoundException(customerId);
        if (!customer.IsActive)
            throw new InactiveCustomerException("An order cannot be created for an inactive customer.");
        return await orders.Add(customerId, amount, DateTime.UtcNow);
    }

    public async Task<Order?> Update(int id, decimal? amount)
    {
        var order = await orders.GetById(id);
        if (order is null) return null;
        if (amount is <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        order.Amount = amount ?? order.Amount;
        await orders.Update(order);
        return order;
    }

    public Task<bool> Delete(int id) => orders.Delete(id);
}
