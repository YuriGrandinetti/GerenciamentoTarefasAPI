using GerenciamentoTarefas.Domain.Interfaces;
using GerenciamentoTarefasAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using static GerenciamentoTarefas.Domain.Enumeradores;

namespace GerenciamentoTarefasAPI.Repository
{
    public class TarefasRepository : ITarefasRepository
    {
        private readonly GerenciamentoTarefasContext _context;
        private readonly GerenciamentoTarefasAPI.Services.RabbitMQService _rabbitMQService;
        private readonly ILogger<TarefasRepository> _logger;

        public TarefasRepository(GerenciamentoTarefasContext context, 
            GerenciamentoTarefasAPI.Services.RabbitMQService rabbitMQService,
            ILogger<TarefasRepository> logger)
        {
            _context = context;
            _rabbitMQService = rabbitMQService;
            _logger = logger;

        }

        public async Task<IEnumerable<Tarefa>> ObterTarefas()
        {
            try
            {
                return await _context.Tarefas.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter as tarefas.");
                throw;
            }


        }

        public async Task<Tarefa> ObterTarefaPorId(int id)
        {
            try
            {
                return await _context.Tarefas.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter tarefa por ID: {Id}", id);
                throw;
            }
        }

        public async Task CriarTarefa(Tarefa novaTarefa)
        {

            try
            {
                _context.Tarefas.Add(novaTarefa);
                await _context.SaveChangesAsync();

                // Enviar mensagem para o RabbitMQ
                _rabbitMQService.EnviarMensagem("task_queue", $"Nova tarefa criada: {novaTarefa.Descricao}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar tarefa: {Descricao}", novaTarefa.Descricao);
                throw;
            }
        }

        public async Task AtualizarTarefa(Tarefa tarefaAtualizada)
        {
            try
            {
                _context.Entry(tarefaAtualizada).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Enviar mensagem para o RabbitMQ sobre a atualização da tarefa
                _rabbitMQService.EnviarMensagem("task_queue", $"Tarefa atualizada: {tarefaAtualizada.Descricao}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar tarefa: {Descricao}", tarefaAtualizada.Descricao);
                throw;
            }
        }

        public async Task ExcluirTarefa(Tarefa tarefa)
        {
            try
            {
                _context.Tarefas.Remove(tarefa);
                await _context.SaveChangesAsync();

                // Enviar mensagem para o RabbitMQ sobre a exclusão da tarefa
                _rabbitMQService.EnviarMensagem("task_queue", $"Tarefa excluída: {tarefa.Descricao}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir tarefa: {Descricao}", tarefa.Descricao);
                throw;
            }
        }

        public async Task<IEnumerable<Tarefa>> ObterTarefasPorUsuario(int usuarioid)
        {
            try
            {
                return await _context.Tarefas
                                     .Where(t => t.usuarioid == usuarioid)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter tarefas por usuário ID: {UsuarioId}", usuarioid);
                throw;
            }
        }

        public async Task<IEnumerable<Tarefa>> ObterTarefasPorUsuarioId(int usuarioId)
        {
            try
            {
                return await _context.Tarefas
                                     .Where(t => t.Status == StatusTarefa.Pendente.ToString())
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter tarefas pendentes.");
                throw;
            }
        }

        public async Task<IEnumerable<Tarefa>> ObterTarefasPendentes()
        {
            try
            {
                return await _context.Tarefas
                                     .Where(t => t.Status == StatusTarefa.Concluida.ToString())
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter tarefas concluídas.");
                throw;
            }
        }

        public async Task<IEnumerable<Tarefa>> ObterTarefasConcluidas()
        {
            return  await _context.Tarefas
                .Where(t => t.Status == StatusTarefa.Concluida.ToString())
                .ToListAsync();
        }

        public async Task AdicionarTarefa(Tarefa novaTarefa)
        {
            try
            {
                _context.Tarefas.Add(novaTarefa);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar tarefa: {Descricao}", novaTarefa.Descricao);
                throw;
            }
        }

        public async Task RemoverTarefa(Tarefa tarefaExistente)
        {
            try
            {
                _context.Tarefas.Remove(tarefaExistente);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover tarefa: {Descricao}", tarefaExistente.Descricao);
                throw;
            }
        }
    }
}
