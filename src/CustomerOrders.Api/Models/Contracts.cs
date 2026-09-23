using System.ComponentModel.DataAnnotations;

public sealed record CreateCustomer(string Name, bool IsActive = true);
public sealed record UpdateCustomer(string? Name, bool? IsActive);
public sealed record CreateOrder([Range(0.01, 1_000_000_000)]decimal Amount);
public sealed record UpdateOrder(decimal? Amount);
public sealed record Problem(string Code, string Detail);
