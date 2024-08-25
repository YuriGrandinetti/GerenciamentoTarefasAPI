using GerenciamentoTarefas.Domain;
using GerenciamentoTarefas.Domain.Interfaces;

namespace GerenciamentoTarefas.Services
{
    public class PerfilUsuarioService
    {
        private readonly IPerfilUsuarioRepository _perfilUsuarioRepository;

        public PerfilUsuarioService(IPerfilUsuarioRepository perfilUsuarioRepository)
        {
            _perfilUsuarioRepository = perfilUsuarioRepository;
        }

        public async Task<IEnumerable<PerfilUsuario>> GetPerfisUsuariosAsync()
        {
            return await _perfilUsuarioRepository.GetPerfisUsuariosAsync();
        }

        public async Task<PerfilUsuario> GetPerfilUsuarioByIdAsync(int id)
        {
            return await _perfilUsuarioRepository.GetPerfilUsuarioByIdAsync(id);
        }

        public async Task<PerfilUsuario> CreatePerfilUsuarioAsync(PerfilUsuario perfilUsuario)
        {
            return await _perfilUsuarioRepository.CreatePerfilUsuarioAsync(perfilUsuario);
        }

        public async Task<bool> DeletePerfilUsuarioAsync(int id)
        {
            return await _perfilUsuarioRepository.DeletePerfilUsuarioAsync(id);
        }
    }
}
