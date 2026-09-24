namespace CustomerOrders.Domain.Exceptions;

public class CustomerNotFoundException(int id): 
    BusinessRuleException(BusinessRuleCodes.CustomerNotFound, $"Customer not found with this id ({id})")
{
    
}
