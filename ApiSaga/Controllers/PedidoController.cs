using ApiSaga.Models;
using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;
using Shared;

namespace ApiSaga.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController(IBus bus) : ControllerBase
{

    [HttpPost("criar")]
    public async Task<IActionResult> CriarPedido([FromBody] PedidoRequest request)
    {
        var message = new PedidoCriado(Guid.NewGuid(), request.EmailCliente, request.Valor, DateTime.Now);

        await bus.Send(message);

        return Ok(new { Status = "Pedido enviado para pagamento" });
    }
}
