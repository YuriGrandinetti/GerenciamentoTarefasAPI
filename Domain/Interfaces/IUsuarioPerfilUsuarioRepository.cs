namespace GerenciamentoTarefas.Domain.Interfaces
{
    public interface IUsuarioPerfilUsuarioRepository
    {
        Task AddPerfilToUsuarioAsync(int usuarioId, int perfilUsuarioId);
        Task RemovePerfilFromUsuarioAsync(int usuarioId, int perfilUsuarioId);
    }
}
