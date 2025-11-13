using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Shared;

namespace ApiSaga.Saga;

public class PedidoSaga(IBus bus, ILogger<PedidoSaga> logger) : Saga<PedidoSagaData>,
                                                                IAmInitiatedBy<PedidoCriado>,
                                                                IHandleMessages<PedidoEnviado>,
                                                                IHandleMessages<EnviarEmail>,
                                                                IHandleMessages<PedidoFinalizado>
{

    protected override void CorrelateMessages(ICorrelationConfig<PedidoSagaData> config)
    {
        config.Correlate<PedidoCriado>(m => m.PedidoId, s => s.PedidoId);
        config.Correlate<PedidoEnviado>(m => m.PedidoId, s => s.PedidoId);
        config.Correlate<EnviarEmail>(m => m.PedidoId, s => s.PedidoId);
        config.Correlate<PedidoFinalizado>(m => m.PedidoId, s => s.PedidoId);
    }

    public async Task Handle(PedidoCriado message)
    {
        if (!IsNew) return;

        logger.LogInformation("1- Iniciando saga para o pedido {PedidoId}", message.PedidoId);

        var aprovado = !string.IsNullOrEmpty(message.EmailCliente);

        await bus.Send(new PedidoEnviado(message.PedidoId,message.Valor, aprovado, message.EmailCliente, DateTime.Now));
    }

    public async Task Handle(PedidoEnviado message)
    {
        if (Data.PedidoCriado)
        {
            logger.LogInformation("Pagamento já processado para {PedidoId}, ignorando.", message.PedidoId);
            return;
        }

        Data.PedidoCriado = message.Aprovado;

        logger.LogInformation("2- Pagamento processado para o pedido {PedidoId}", message.PedidoId);
    }

    public async Task Handle(EnviarEmail message)
    {
        if (Data.PedidoCriado)
        {
            logger.LogInformation("Email já enviado para {PedidoId}, ignorando.", message.PedidoId);
            return;
        }

        Data.EmailEnviado = !string.IsNullOrEmpty(message.EmailCliente);

        logger.LogInformation("3- Enviando email de confirmação para o pedido {PedidoId}", message.PedidoId);
    }

    public async Task Handle(PedidoFinalizado message)
    {
        if (Data.EmailEnviado)
        {
            logger.LogInformation("Pedido já finalizado para {PedidoId}, ignorando.", message.PedidoId);
            return;
        }

        Data.PedidoFinalizado = message.Status == EPedidoStatus.OK;

        logger.LogInformation("4- Pedido {PedidoId} finalizado com sucesso!", message.PedidoId);

        MarkAsComplete();
    }
}