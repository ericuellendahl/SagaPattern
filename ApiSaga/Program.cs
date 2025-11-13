using ApiSaga.Configurations;
using ApiSaga.Saga;
using ApiSaga.Services;
using Rebus.Config;
using Rebus.Persistence.InMem;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IEmailService, EmailService>();

var rabbitSettings = builder.Configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>()!;

var amqpUrl =
    $"{(rabbitSettings.SslEnabled ? "amqps" : "amqp")}://" +
    $"{rabbitSettings.UserName}:{rabbitSettings.Password}@" +
    $"{rabbitSettings.HostName}:{rabbitSettings.Port}/{rabbitSettings.VirtualHost}";

builder.Services.AddRebus(config => config
    .Logging(l => l.ColoredConsole(Rebus.Logging.LogLevel.Info))
    .Transport(t => t.UseRabbitMq(amqpUrl, "pedido-queue"))
    .Routing(r => r.TypeBased()
        .Map<PedidoCriado>("pedido-queue")
        .Map<PedidoEnviado>("pedido-queue")
        .Map<EnviarEmail>("pedido-queue")
        .Map<PedidoFinalizado>("pedido-queue")
        .Map<EnviarPagamento>("pagamento-queue"))
    .Sagas(s => s.StoreInMemory())
    .Options(o => o.RetryStrategy(maxDeliveryAttempts: 1)));

builder.Services.AutoRegisterHandlersFromAssemblyOf<PedidoSaga>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
