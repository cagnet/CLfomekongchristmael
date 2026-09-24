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
    public async Task<ActionResult<IEnumerable<Customer>>> ReadAll(CancellationToken cancellationToken) => Ok(await service.ReadAll(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Customer>> ReadOne(int id, CancellationToken cancellationToken) => 
        await service.ReadOne(id, cancellationToken) is { } customer ? Ok(customer) : NotFound();

    [HttpGet("{id:int}/orders")]
    public async Task<ActionResult<IEnumerable<Order>>> ReadOrders(int id, CancellationToken cancellationToken) => 
        await service.ReadOrders(id, cancellationToken) is { } orders ? Ok(orders) : NotFound();

    [HttpPost]
    public async Task<ActionResult<Customer>> Create(CreateCustomer request, CancellationToken cancellationToken)
    {
        var customer = await service.Create(request.Name, request.FirstName, 
            request.Email, request.Address, request.IsActive, cancellationToken);
        return CreatedAtAction(nameof(ReadOne), new { id = customer.Id }, customer);
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<Customer>> Update(int id, UpdateCustomer request, CancellationToken cancellationToken)
    {

        var customer = await service.Update(id, request.Name, request.IsActive, cancellationToken);
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await service.Delete(id, cancellationToken) ? NoContent() : NotFound();

    }
}
