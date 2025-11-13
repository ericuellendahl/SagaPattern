using ApiSagaPayment.Models;
using Newtonsoft.Json;
using Rebus.Bus;
using Rebus.Handlers;
using Shared;

namespace ApiSagaPayment.Handlers;

public class EnviarPagamentoAprovadoHandler(IBus bus, ILogger<EnviarPagamentoAprovadoHandler> logger) : IHandleMessages<EnviarPagamentoAprovado>
{
    public async Task Handle(EnviarPagamentoAprovado message)
    {
        logger.LogInformation("📦 Recebido PedidoCriado: {PedidoId}", message.PedidoId);

        var pedido = new PedidoPagamento
        {
            PedidoId = message.PedidoId,
            Valor = message.Valor,
            EmailCliente = message.EmailCliente,
            Status = "Aprovado",
            DataCriacao = DateTime.UtcNow
        };

        logger.LogInformation(JsonConvert.SerializeObject(pedido));

        //await bus.Send(new PagamentoProcessado(message.PedidoId));

        logger.LogInformation("✅ Pedido salvo com sucesso no banco.");
    }
}
