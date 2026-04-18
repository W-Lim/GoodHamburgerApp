using Microsoft.AspNetCore.Mvc;
using GoodHamburger.Api.Services;
using GoodHamburger.Models;

namespace GoodHamburger.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _service;
    public OrdersController(OrderService service) => _service = service;

    [HttpGet("menu")]
    public IActionResult GetMenu() => Ok(OrderService.Menu);

    [HttpPost]
    public IActionResult Create([FromBody] OrderRequest request)
    {
        // 1. Verifica se a lista de IDs não veio vazia
        if (request == null || request.ItemIds == null || !request.ItemIds.Any())
        {
            return BadRequest("O pedido deve conter pelo menos um item.");
        }

        // 2. Chama a validação que ajustamos acima
        var validation = _service.ValidateOrder(request.ItemIds);

        if (!validation.Valid)
        {
            return BadRequest(validation.Message); // Retorna 400 com a mensagem clara
        }

        // 3. Cria o pedido e retorna 200 OK
        var newOrder = _service.CreateOrder(request.ItemIds);
        return Ok(newOrder);
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_service.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var order = _service.GetById(id);
        return order == null ? NotFound() : Ok(order);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id) => _service.Delete(id) ? NoContent() : NotFound();
}