namespace ApiSaga.Services;

public interface IEmailService
{
    Task EnviarEmailAsync(string destinatario, string assunto, string corpo);
}

public class EmailService(ILogger<EmailService> logger) : IEmailService
{
    public Task EnviarEmailAsync(string destinatario, string assunto, string corpo)
    {
        logger.LogInformation("2-3 Enviando email para {Destinatario} com assunto '{Assunto}' e corpo '{Corpo}'",
            destinatario, assunto, corpo);

        return Task.CompletedTask;
    }
}
