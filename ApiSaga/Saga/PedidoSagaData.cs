using Rebus.Sagas;

namespace ApiSaga.Saga;

public class PedidoSagaData : SagaData
{
    public Guid PedidoId { get; set; }
    public string EmailCliente { get; set; } = string.Empty;
    public bool PedidoCriado { get; set; }
    public bool EmailEnviado { get; set; }
    public bool PedidoFinalizado { get; set; }
    public decimal Valor { get; set; }
}
