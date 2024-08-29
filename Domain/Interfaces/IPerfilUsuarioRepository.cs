namespace GerenciamentoTarefas.Domain.Interfaces
{
    public interface IPerfilUsuarioRepository
    {
        Task<IEnumerable<PerfilUsuario>> GetPerfisUsuariosAsync();
        Task<PerfilUsuario> GetPerfilUsuarioByIdAsync(int id);
        Task<PerfilUsuarioDto> CreatePerfilUsuarioAsync(PerfilUsuarioDto perfilUsuario);
        Task<bool> DeletePerfilUsuarioAsync(int id);
    }
}
