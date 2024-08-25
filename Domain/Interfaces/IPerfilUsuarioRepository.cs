namespace GerenciamentoTarefas.Domain.Interfaces
{
    public interface IPerfilUsuarioRepository
    {
        Task<IEnumerable<PerfilUsuario>> GetPerfisUsuariosAsync();
        Task<PerfilUsuario> GetPerfilUsuarioByIdAsync(int id);
        Task<PerfilUsuario> CreatePerfilUsuarioAsync(PerfilUsuario perfilUsuario);
        Task<bool> DeletePerfilUsuarioAsync(int id);
    }
}
