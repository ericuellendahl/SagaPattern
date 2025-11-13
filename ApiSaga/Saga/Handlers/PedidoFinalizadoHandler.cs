using Rebus.Bus;
using Rebus.Handlers;
using Shared;

namespace ApiSaga.Saga.Handlers;

public class PedidoFinalizadoHandler(IBus bus, ILogger<PedidoFinalizadoHandler> logger) : IHandleMessages<PedidoFinalizado>
{
    public async Task Handle(PedidoFinalizado message)
    {
        if (message?.PedidoId == null)
        {
            logger.LogError("PedidoId é nulo. Não é possível enviar o email.");
            return;
        }

        logger.LogInformation("4-1 Pedido {PedidoId} finalizado com sucesso no serviço de pagamento.", message.PedidoId);

        await bus.Send(new EnviarPagamentoAprovado(message.PedidoId, message.Valor, message.EmailCliente, DateTime.Now));
    }
}
