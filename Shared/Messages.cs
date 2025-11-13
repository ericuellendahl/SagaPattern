namespace Shared;

// Pedido foi criado no sistema de pedidos
public record PedidoCriado(
    Guid PedidoId,
    string EmailCliente,
    decimal Valor,
    DateTime DataCriacao
);

// Pagamento foi processado pelo sistema de pagamento
public record PedidoEnviado(
    Guid PedidoId,
    decimal Valor,
    bool Aprovado,
    string EmailCliente,
    DateTime DataProcessamento
);

// Solicitação para enviar e-mail de confirmação
public record EnviarEmail(
    Guid PedidoId,
    decimal Valor,
    string EmailCliente,
    string Mensagem
);

// Pedido finalizado com sucesso
public record PedidoFinalizado(
    Guid PedidoId,
    decimal Valor,
    EPedidoStatus Status,
    string EmailCliente,
    DateTime DataFinalizacao
);

// Pedido aprovado e enviado ao serviço de pagamento
public record EnviarPagamento(
    Guid PedidoId,
    decimal Valor,
    string EmailCliente,
    DateTime DataEnvio
);


