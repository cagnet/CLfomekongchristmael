using CustomerOrders.Business.Entities;

namespace CustomerOrders.Business.Repositories;

public interface IOrderRepository
{
    Task<IReadOnlyCollection<Order>> GetAll();
    Task<IReadOnlyCollection<Order>> GetByCustomerId(int customerId);
    Task<Order?> GetById(int id);
    Task<Order> Add(int customerId, decimal amount, DateTime createdAt);
    Task Update(Order order);
    Task<bool> Delete(int id);
    Task<bool> ExistsForCustomer(int customerId);
}
