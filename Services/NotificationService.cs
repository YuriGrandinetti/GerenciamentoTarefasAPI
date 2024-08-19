using GerenciamentoTarefasAPI.Services;

public class NotificationService
{
    private readonly RabbitMQService _rabbitMQService;

    public NotificationService(RabbitMQService rabbitMQService)
    {
        _rabbitMQService = rabbitMQService;
    }

    public void EnviarNotificacao(string message)
    {
        _rabbitMQService.EnviarMensagem("notification_queue", message);
    }

    public void EnviarNotificacaoDeTarefaCriada(string tarefaDescricao)
    {
        string message = $"Notificação: Uma nova tarefa foi criada: {tarefaDescricao}";
        EnviarNotificacao(message);
    }

    public void EnviarNotificacaoDeTarefaConcluida(string tarefaDescricao)
    {
        string message = $"Notificação: A tarefa '{tarefaDescricao}' foi concluída.";
        EnviarNotificacao(message);
    }

<<<<<<< HEAD
    public void EnviarNotificacaoDeTarefaAlterada(string tarefaDescricao)
    {
        string message = $"Notificação: A tarefa '{tarefaDescricao}' foi alterada.";
        EnviarNotificacao(message);
    }


=======
>>>>>>> dc185a08486fd19f43707bdaa4f524e38bd8532b

}
