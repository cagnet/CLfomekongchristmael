

using CustomerOrders.Domain.Models;

namespace CustomerOrders.Business.Contracts;

public interface ICustomerRepository
{
    Task<IReadOnlyCollection<Customer>> GetAll(CancellationToken cancellationToken);
    Task<Customer?> GetById(int id, CancellationToken cancellationToken);
    Task<Customer> Add(string name, string firstName, String email,
        String address, bool isActive, CancellationToken cancellationToken);
    Task Update(Customer customer, CancellationToken cancellationToken);
    Task<bool> Delete(int id, CancellationToken cancellationToken);
}
