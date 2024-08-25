namespace GerenciamentoTarefas.Domain.Interfaces
{
    public interface IRabbitMQService
    {
        void EnviarMensagem(string queue, string mensagem);
    }
}

    
