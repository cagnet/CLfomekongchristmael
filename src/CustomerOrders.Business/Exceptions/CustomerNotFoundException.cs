namespace CustomerOrders.Business.Exceptions;

public class CustomerNotFoundException(int id): 
    BusinessRuleException("customer_not_found", $"Customer not found with this id ({id})")
{
    
}