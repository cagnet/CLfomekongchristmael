namespace CustomerOrders.Business.Exceptions;

public class InactiveCustomerException(): 
    BusinessRuleException("inactive_customer", "An order cannot be created for an inactive customer.")
{
    
}