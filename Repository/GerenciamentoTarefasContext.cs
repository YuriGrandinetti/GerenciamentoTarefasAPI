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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura a relação entre Tarefa e Usuario
            modelBuilder.Entity<Tarefa>()
                .HasOne(t => t.Usuario)             // Cada Tarefa tem um Usuario
                .WithMany(u => u.Tarefas)           // Um Usuario pode ter muitas Tarefas
                .HasForeignKey(t => t.usuarioid);   // Chave estrangeira em Tarefa é UsuarioId
        }
    }
}
