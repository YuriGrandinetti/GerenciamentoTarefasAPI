using GerenciamentoTarefas.Domain;
using GerenciamentoTarefas.Domain.Interfaces;
using GerenciamentoTarefasAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoTarefas.Repository
{
    public class UsuarioPerfilUsuarioRepository : IUsuarioPerfilUsuarioRepository
    {
        private readonly GerenciamentoTarefasContext _context;

        public UsuarioPerfilUsuarioRepository(GerenciamentoTarefasContext context)
        {
            _context = context;
        }

        public async Task AddPerfilToUsuarioAsync(int usuarioId, int perfilUsuarioId)
        {
            var usuarioPerfil = new UsuarioPerfilUsuario
            {
                UsuarioId = usuarioId,
                IdPerfilUsuario = perfilUsuarioId
            };
            _context.UsuariosPerfisUsuarios.Add(usuarioPerfil);
            await _context.SaveChangesAsync();
        }

        public async Task RemovePerfilFromUsuarioAsync(int usuarioId, int perfilUsuarioId)
        {
            var usuarioPerfil = await _context.UsuariosPerfisUsuarios
                .FirstOrDefaultAsync(up => up.UsuarioId == usuarioId && up.IdPerfilUsuario == perfilUsuarioId);

            if (usuarioPerfil != null)
            {
                _context.UsuariosPerfisUsuarios.Remove(usuarioPerfil);
                await _context.SaveChangesAsync();
            }
        }

    }
}
