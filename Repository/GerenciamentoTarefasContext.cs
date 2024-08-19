using Microsoft.EntityFrameworkCore;
using GerenciamentoTarefasAPI.Models;

namespace GerenciamentoTarefasAPI.Repository
{
    public class GerenciamentoTarefasContext : DbContext
    {
        public GerenciamentoTarefasContext(DbContextOptions<GerenciamentoTarefasContext> options)
            : base(options)
        {
        }

        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}