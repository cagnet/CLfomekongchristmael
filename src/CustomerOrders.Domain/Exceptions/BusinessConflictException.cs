namespace CustomerOrders.Domain.Exceptions;

public class BusinessConflictException(String code, String message)
    : BusinessRuleException(code, message)
{
    
}