# 🧩 Projeto de Orquestração com Rebus, RabbitMQ e Sagas (.NET 8)

Este projeto demonstra como orquestrar o fluxo entre diferentes serviços (ex: **Pedidos**, **Pagamentos** e **E-mails**) utilizando o **Rebus**, um framework leve para mensageria e Sagas no .NET.

## 🚀 Tecnologias Utilizadas

- **.NET 8 / C# 12**
- **Rebus** → abstração de mensageria e Sagas
- **RabbitMQ** → transporte de mensagens
- **Rebus.Sagas** → controle de estado e correlação de mensagens
- **Rebus.RabbitMq** → integração com RabbitMQ
- **Dependency Injection padrão do ASP.NET Core**

---

## 📦 Arquitetura

O sistema é dividido em **duas APIs**:

1. **ApiSaga (Pedidos)**  
   Responsável por criar pedidos e orquestrar o fluxo via Saga.
   - Cria o pedido (`PedidoCriado`)
   - Publica mensagem para processamento de pagamento
   - Aguarda retorno (`PagamentoProcessado`)
   - Envia e-mail de confirmação (`EnviarEmail`)
   - Finaliza o pedido (`PedidoFinalizado`)
   - Notifica o serviço de pagamento (`EnviarPagamentoAprovado`)

2. **ApiPagamento (Pagamentos)**  
   Recebe mensagens da API de Pedidos, processa o pagamento e envia de volta a resposta.

---

## 🧠 Fluxo de Mensagens

```text
Pedido → Pagamento → E-mail → Pedido Finalizado
