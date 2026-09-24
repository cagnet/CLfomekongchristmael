
using CustomerOrders.Domain.Models;

namespace CustomerOrders.Business.Contracts;

public interface IOrderRepository
{
    Task<IReadOnlyCollection<Order>> GetAll(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Order>> GetByCustomerId(int customerId, CancellationToken cancellationToken);
    Task<Order?> GetById(int id, CancellationToken cancellationToken);
    Task<Order> Add(int customerId, decimal amount, DateTime createdAt, CancellationToken cancellationToken);
    Task Update(Order order, CancellationToken cancellationToken);
    Task<bool> Delete(int id, CancellationToken cancellationToken);
    Task<bool> ExistsForCustomer(int customerId, CancellationToken cancellationToken);
}
