using System.ComponentModel.DataAnnotations;

namespace CustomerOrders.Business.Entities;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public String Address { get; set; }
    public bool IsActive { get; set; }

}
