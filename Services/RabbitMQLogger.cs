using GerenciamentoTarefasAPI.Services;

public class RabbitMQLogger
{
    private readonly RabbitMQService _rabbitMQService;

    public RabbitMQLogger(RabbitMQService rabbitMQService)
    {
        _rabbitMQService = rabbitMQService;
    }

    public void LogInformation(string message)
    {
        _rabbitMQService.EnviarMensagem("log_queue", $"INFO: {message}");
    }

    public void LogError(string message)
    {
        _rabbitMQService.EnviarMensagem("log_queue", $"ERROR: {message}");
    }
}

