using System.ComponentModel.DataAnnotations;

public sealed record CreateCustomer(
    [StringLength(30, MinimumLength = 2)] string Name, 
    [StringLength(20, MinimumLength = 2)] string FirstName, 
    [EmailAddress] [StringLength(150)] String Email,
    [StringLength(20, MinimumLength = 6)] String Address, 
    bool IsActive = true);
public sealed record UpdateCustomer([StringLength(30, MinimumLength = 2)] string? Name, bool? IsActive);
public sealed record CreateOrder([Range(0.01, 1_000_000_000)]decimal Amount);
public sealed record UpdateOrder([Range(0.01, 1_000_000_000)]decimal? Amount);
public sealed record Problem(string Code, string Detail);
