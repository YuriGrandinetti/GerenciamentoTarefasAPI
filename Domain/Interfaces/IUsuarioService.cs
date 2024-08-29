using GerenciamentoTarefasAPI.Models;

namespace GerenciamentoTarefas.Domain.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> RegistrarUsuario(UsuariosCreatedDto novoUsuario);
        Task<Usuario> LoginUsuario(string email, string senha);
        string GerarToken(Usuario usuario);
        Task<Usuario> ObterUsuarioPorId(int id);
        Task<List<UauarioDto>> ObterTodosUsuarios();
        Task<Usuario> UpdateUsuario(int id, UsuarioAtualizadoDto usuarioAtualizado);
    }
}
