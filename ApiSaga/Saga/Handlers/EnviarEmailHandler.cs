using ApiSaga.Services;
using Rebus.Bus;
using Rebus.Handlers;
using Shared;

namespace ApiSaga.Saga.Handlers;

public class EnviarEmailHandler(IBus bus, IEmailService email, ILogger<EnviarEmailHandler> logger) : IHandleMessages<EnviarEmail>
{
    public async Task Handle(EnviarEmail message)
    {

        if (message?.PedidoId == null)
        {
            logger.LogError("PedidoId é nulo. Não é possível enviar o email.");
            return;
        }
        logger.LogInformation("3-1 Processando pagamento para o pedido {PedidoId}", message.PedidoId);

        await email.EnviarEmailAsync(message.EmailCliente,
                                    "Pedido confirmado!",
                                    $"Seu pedido {message.PedidoId} gerado com sucesso estamos confirmando o pagamento!.");

        var status = message?.PedidoId != null ? EPedidoStatus.OK : EPedidoStatus.Falha;

        await bus.Send(new PedidoFinalizado(message.PedidoId, message.Valor, status, message.EmailCliente, DateTime.Now));
    }
}

