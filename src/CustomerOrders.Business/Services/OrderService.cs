using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Exceptions;
using CustomerOrders.Business.Repositories;

namespace CustomerOrders.Business.Services;

public sealed class OrderService(ICustomerRepository customers, IOrderRepository orders)
{
    public Task<IReadOnlyCollection<Order>> ReadAll(CancellationToken cancellationToken) => orders.GetAll(cancellationToken);
    public Task<Order?> ReadOne(int id, CancellationToken cancellationToken) => orders.GetById(id, cancellationToken);

    public async Task<Order> Create(int customerId, decimal amount, CancellationToken cancellationToken)
    {
        var customer = await customers.GetById(customerId, cancellationToken);
        if (customer is null) 
            throw new CustomerNotFoundException(customerId);
        if (!customer.IsActive)
            throw new InactiveCustomerException("An order cannot be created for an inactive customer.");
        return await orders.Add(customerId, amount, DateTime.UtcNow, cancellationToken);
    }

    public async Task<Order?> Update(int id, decimal? amount, CancellationToken cancellationToken)
    {
        var order = await orders.GetById(id, cancellationToken);
        if (order is null) return null;
        if (amount is <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        order.Amount = amount ?? order.Amount;
        await orders.Update(order, cancellationToken);
        return order;
    }

    public Task<bool> Delete(int id, CancellationToken cancellationToken) => orders.Delete(id, cancellationToken);
}
