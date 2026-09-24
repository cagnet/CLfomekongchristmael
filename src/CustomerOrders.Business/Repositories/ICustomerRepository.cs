using CustomerOrders.Business.Entities;

namespace CustomerOrders.Business.Repositories;

public interface ICustomerRepository
{
    Task<IReadOnlyCollection<Customer>> GetAll();
    Task<Customer?> GetById(int id);
    Task<Customer> Add(string name, string firstName, String email,
        String address, bool isActive);
    Task Update(Customer customer);
    Task<bool> Delete(int id);
}
