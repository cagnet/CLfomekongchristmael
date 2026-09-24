namespace CustomerOrders.Business.Exceptions;

public class BusinessConflictException(String code, String message)
    : BusinessRuleException(code, message)
{
    
}