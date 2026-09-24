using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Exceptions;
using CustomerOrders.Business.Repositories;

namespace CustomerOrders.Business.Services;

public sealed class CustomerService(ICustomerRepository customers, IOrderRepository orders)
{
    public Task<IReadOnlyCollection<Customer>> ReadAll() => customers.GetAll();
    public Task<Customer?> ReadOne(int id) => customers.GetById(id);
    public async Task<IReadOnlyCollection<Order>?> ReadOrders(int id) => await customers.GetById(id) is null ? null : await orders.GetByCustomerId(id);

    public Task<Customer> Create(string name, string firstName, String email,
        String address, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        return customers.Add(name.Trim(), firstName.Trim(), email.Trim(),
            address.Trim() ,isActive);
    }

    public async Task<Customer?> Update(int id, string? name, bool? isActive)
    {
        var customer = await customers.GetById(id);
        if (customer is null) return null;
        if (name is not null && string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(name));
        customer.Name = name?.Trim() ?? customer.Name;
        customer.IsActive = isActive ?? customer.IsActive;
        await customers.Update(customer);
        return customer;
    }

    public async Task<bool> Delete(int id)
    {
        if (await orders.ExistsForCustomer(id))
            throw new BusinessRuleException("customer_has_orders", "A customer with orders cannot be deleted.");
        return await customers.Delete(id);
    }
}
