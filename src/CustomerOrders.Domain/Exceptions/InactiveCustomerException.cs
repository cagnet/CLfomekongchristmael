namespace CustomerOrders.Domain.Exceptions;

public class InactiveCustomerException(String message): 
    BusinessRuleException(BusinessRuleCodes.InactiveCustomer, message)
{
    public InactiveCustomerException() : this("Customer is inactive"){}
}