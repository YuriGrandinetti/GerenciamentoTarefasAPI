using GerenciamentoTarefas.Domain;
using GerenciamentoTarefas.Domain.Interfaces;
using GerenciamentoTarefasAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoTarefas.Repository
{
    public class PerfilUsuarioRepository : IPerfilUsuarioRepository
    {
        private readonly GerenciamentoTarefasContext _context;

        public PerfilUsuarioRepository(GerenciamentoTarefasContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PerfilUsuario>> GetPerfisUsuariosAsync()
        {
            return await _context.PerfisUsuarios.ToListAsync();
        }

        public async Task<PerfilUsuario> GetPerfilUsuarioByIdAsync(int id)
        {
            return await _context.PerfisUsuarios.FindAsync(id);
        }

        public async Task<PerfilUsuario> CreatePerfilUsuarioAsync(PerfilUsuario perfilUsuario)
        {
            _context.PerfisUsuarios.Add(perfilUsuario);
            await _context.SaveChangesAsync();
            return perfilUsuario;
        }

        public async Task<bool> DeletePerfilUsuarioAsync(int id)
        {
            var perfilUsuario = await _context.PerfisUsuarios.FindAsync(id);
            if (perfilUsuario == null) return false;

            _context.PerfisUsuarios.Remove(perfilUsuario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
