using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Exceptions;
using CustomerOrders.Business.Repositories;

namespace CustomerOrders.Business.Services;

public sealed class CustomerService(ICustomerRepository customers, IOrderRepository orders)
{
    public Task<IReadOnlyCollection<Customer>> ReadAll(CancellationToken cancellationToken) => customers.GetAll(cancellationToken);
    public Task<Customer?> ReadOne(int id, CancellationToken cancellationToken) => customers.GetById(id, cancellationToken);
    public async Task<IReadOnlyCollection<Order>?> ReadOrders(int id, CancellationToken cancellationToken) => await customers.GetById(id, cancellationToken) is null ?
        null : await orders.GetByCustomerId(id, cancellationToken);

    public Task<Customer> Create(string name, string firstName, String email,
        String address, bool isActive, CancellationToken cancellationToken)
    {
        return customers.Add(name.Trim(), firstName.Trim(), email.Trim(),
            address.Trim() ,isActive, cancellationToken);
    }

    public async Task<Customer?> Update(int id, string? name, bool? isActive, CancellationToken cancellationToken)
    {
        var customer = await customers.GetById(id, cancellationToken);
        if (customer is null) return null;
        if (name is not null && string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(name));
        customer.Name = name?.Trim() ?? customer.Name;
        customer.IsActive = isActive ?? customer.IsActive;
        await customers.Update(customer, cancellationToken);
        return customer;
    }

    public async Task<bool> Delete(int id, CancellationToken cancellationToken)
    {
        if (await orders.ExistsForCustomer(id, cancellationToken))
            throw new BusinessConflictException(BusinessRuleCodes.Conflicts.CustomerHasOrders, "A customer with orders cannot be deleted.");
        return await customers.Delete(id, cancellationToken);
    }
}
