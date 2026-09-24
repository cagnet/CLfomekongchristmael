namespace CustomerOrders.Business.Exceptions;

public static class BusinessRuleCodes
{
    public const string CustomerNotFound = "customer_not_found";
    public const string InactiveCustomer = "inactive_customer";
    
    public static class Conflicts
    {
        public const string CustomerHasOrders = "customer_has_orders";
    }
    
}