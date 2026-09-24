using Microsoft.AspNetCore.Mvc;
using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Exceptions;
using CustomerOrders.Business.Services;

[ApiController]
[Route("orders")]
public sealed class OrdersController(OrderService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> ReadAll(CancellationToken cancellationToken) => Ok(await service.ReadAll(cancellationToken));

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<Order>> ReadOne(int id, CancellationToken cancellationToken) 
        => await service.ReadOne(id, cancellationToken) is { } order ? Ok(order) : NotFound();

    [HttpPost("/customers/{customerId:int:min(1)}/orders")]
    public async Task<ActionResult<Order>> Create(int customerId, CreateOrder request, CancellationToken cancellationToken)
    {
        var order = await service.Create(customerId, request.Amount, cancellationToken);
        return CreatedAtAction(nameof(ReadOne), new { id = order.Id }, order);
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<ActionResult<Order>> Update(int id, UpdateOrder request, CancellationToken cancellationToken)
    {
        
        var order = await service.Update(id, request.Amount, cancellationToken);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken) 
        => await service.Delete(id, cancellationToken) ? NoContent() : NotFound();
}
