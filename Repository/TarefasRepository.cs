using GerenciamentoTarefasAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace GerenciamentoTarefasAPI.Repository
{
    public class TarefasRepository
    {
        private readonly GerenciamentoTarefasContext _context;
        private readonly GerenciamentoTarefasAPI.Services.RabbitMQService _rabbitMQService;

        public TarefasRepository(GerenciamentoTarefasContext context, GerenciamentoTarefasAPI.Services.RabbitMQService rabbitMQService)
        {
            _context = context;
            _rabbitMQService = rabbitMQService;
        }

        public async Task<IEnumerable<Tarefa>> ObterTarefas()
        {
            return await _context.Tarefas.ToListAsync();
        }

        public async Task<Tarefa> ObterTarefaPorId(int id)
        {
            return await _context.Tarefas.FindAsync(id);
        }

        public async Task CriarTarefa(Tarefa novaTarefa)
        {
            _context.Tarefas.Add(novaTarefa);
            await _context.SaveChangesAsync();

            // Enviar mensagem para o RabbitMQ
            _rabbitMQService.EnviarMensagem("task_queue", $"Nova tarefa criada: {novaTarefa.Descricao}");
        }

        public async Task AtualizarTarefa(Tarefa tarefaAtualizada)
        {
            _context.Entry(tarefaAtualizada).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();

            // Enviar mensagem para o RabbitMQ sobre a atualização da tarefa
            _rabbitMQService.EnviarMensagem("task_queue", $"Tarefa atualizada: {tarefaAtualizada.Descricao}");
        }

        public async Task ExcluirTarefa(Tarefa tarefa)
        {
            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();

            // Enviar mensagem para o RabbitMQ sobre a exclusão da tarefa
            _rabbitMQService.EnviarMensagem("task_queue", $"Tarefa excluída: {tarefa.Descricao}");
        }
    }
}
