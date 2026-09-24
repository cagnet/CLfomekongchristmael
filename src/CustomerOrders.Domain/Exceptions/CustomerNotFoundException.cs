namespace CustomerOrders.Business.Exceptions;

public class CustomerNotFoundException(int id): 
    BusinessRuleException(BusinessRuleCodes.CustomerNotFound, $"Customer not found with this id ({id})")
{
    
}
