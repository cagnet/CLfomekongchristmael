using System.ComponentModel.DataAnnotations;

namespace CustomerOrders.Business.Entities;

public sealed record Customer(
    int Id, 
    string Name, 
    string FirstName,
    string Email,
    String Address,
    bool IsActive
    );
