using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;
using Shared;

namespace ApiSagaPayment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagamentoController(IBus bus) : ControllerBase
{

    [HttpPost("{pedidoId:guid}")]
    public async Task<IActionResult> ProcessarPagamento(Guid pedidoId, decimal valor, string Email)
    {
        Console.WriteLine($"💳 Pagamento para {pedidoId}");
        try
        {
            await bus.Send(new PedidoEnviado(pedidoId, valor, true, Email, DateTime.Now));

            return Ok(new { pedidoId, sucess = true });
        }
        catch
        {
            return BadRequest();
        }
    }
}
