using Microsoft.AspNetCore.Mvc;
using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Exceptions;
using CustomerOrders.Business.Services;

[ApiController]
[Route("orders")]
public sealed class OrdersController(OrderService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> ReadAll() => Ok(await service.ReadAll());

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<Order>> ReadOne(int id) => await service.ReadOne(id) is { } order ? Ok(order) : NotFound();

    [HttpPost("/customers/{customerId:int:min(1)}/orders")]
    public async Task<ActionResult<Order>> Create(int customerId, CreateOrder request)
    {
        var order = await service.Create(customerId, request.Amount);
        return CreatedAtAction(nameof(ReadOne), new { id = order.Id }, order);
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<ActionResult<Order>> Update(int id, UpdateOrder request)
    {
        
        var order = await service.Update(id, request.Amount);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => await service.Delete(id) ? NoContent() : NotFound();
}
