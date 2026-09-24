using Microsoft.AspNetCore.Mvc;
using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Exceptions;
using CustomerOrders.Business.Services;

namespace CustomerOrders.Api.Controllers;


[ApiController]
[Route("customers")]
public sealed class CustomersController(CustomerService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> ReadAll() => Ok(await service.ReadAll());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Customer>> ReadOne(int id) => await service.ReadOne(id) is { } customer ? Ok(customer) : NotFound();

    [HttpGet("{id:int}/orders")]
    public async Task<ActionResult<IEnumerable<Order>>> ReadOrders(int id) => await service.ReadOrders(id) is { } orders ? Ok(orders) : NotFound();

    [HttpPost]
    public async Task<ActionResult<Customer>> Create(CreateCustomer request)
    {
        var customer = await service.Create(request.Name, request.FirstName, request.Email, request.Address, request.IsActive);
        return CreatedAtAction(nameof(ReadOne), new { id = customer.Id }, customer);
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<Customer>> Update(int id, UpdateCustomer request)
    {

        var customer = await service.Update(id, request.Name, request.IsActive);
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            return await service.Delete(id) ? NoContent() : NotFound();
        }
        catch (BusinessRuleException exception)
        {
            return Conflict(new Problem(exception.Code, exception.Message));
        }
    }
}
