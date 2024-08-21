using GerenciamentoTarefasAPI.Models;

namespace GerenciamentoTarefas.Domain.Interfaces
{
    public interface ITarefasRepository
    {
        Task<IEnumerable<Tarefa>> ObterTarefas();
        Task<Tarefa> ObterTarefaPorId(int id);
        Task CriarTarefa(Tarefa novaTarefa);
        Task AtualizarTarefa(Tarefa tarefaAtualizada);
        Task ExcluirTarefa(Tarefa tarefa);
        Task<IEnumerable<Tarefa>> ObterTarefasPorUsuarioId(int usuarioId);
        Task<IEnumerable<Tarefa>> ObterTarefasPorUsuario(int usuarioId);
        Task<IEnumerable<Tarefa>> ObterTarefasPendentes();
        Task<IEnumerable<Tarefa>> ObterTarefasConcluidas();       
        Task AdicionarTarefa(Tarefa novaTarefa);
        Task RemoverTarefa(Tarefa tarefaExistente);
    }
}
