using Rebus.Bus;
using Rebus.Handlers;
using Shared;

namespace ApiSaga.Saga.Handlers;

public class PedidoEnviadoHandler(IBus bus, ILogger<PedidoEnviadoHandler> logger) : IHandleMessages<PedidoEnviado>
{
    public async Task Handle(PedidoEnviado message)
    {
        if (message?.PedidoId == null)
        {
            logger.LogError("PedidoId é nulo. Não é possível enviar o email.");
            return;
        }

        logger.LogInformation("2-1 Processamento do pedido baixa do estoque ou reservar {PedidoId}", message.PedidoId);

        var textoEmail = message.Aprovado
            ? "Seu pedido foi aprovado e está sendo processado."
            : "Seu pedido foi reprovado. Entre em contato com o suporte.";

        await bus.Send(new EnviarEmail(message.PedidoId, message.Valor, message.EmailCliente, textoEmail));
    }
}
