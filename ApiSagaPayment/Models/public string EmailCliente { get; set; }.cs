namespace ApiSagaPayment.Models;

public class PedidoPagamento
{
    public int Id { get; set; }
    public Guid PedidoId { get; set; }
    public string EmailCliente { get; set; }
    public decimal Valor { get; set; }
    public string Status { get; set; } = "Aguardando pagamento";
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}
