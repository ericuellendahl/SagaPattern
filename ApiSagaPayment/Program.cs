using ApiSagaPayment.Configurations;
using Rebus.Config;
using Rebus.Routing.TypeBased;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurações do RabbitMQ
var rabbitSettings = builder.Configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>()!;

var amqpUrl =
    $"{(rabbitSettings.SslEnabled ? "amqps" : "amqp")}://" +
    $"{rabbitSettings.UserName}:{rabbitSettings.Password}@" +
    $"{rabbitSettings.HostName}:{rabbitSettings.Port}/{rabbitSettings.VirtualHost}";

// Configuração do Rebus
builder.Services.AddRebus(config => config
    .Transport(t => t.UseRabbitMq(amqpUrl, "pagamento-queue"))
    .Routing(r => r.TypeBased()
    )
);

builder.Services.AutoRegisterHandlersFromAssemblyOf<Program>();

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
