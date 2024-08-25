using Microsoft.EntityFrameworkCore;
using GerenciamentoTarefasAPI.Models;
using GerenciamentoTarefas.Domain;

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

        public DbSet<PerfilUsuario> PerfisUsuarios { get; set; }
        public DbSet<UsuarioPerfilUsuario> UsuariosPerfisUsuarios { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura a relação entre Tarefa e Usuario
            modelBuilder.Entity<Tarefa>()
                .HasOne(t => t.Usuario)             // Cada Tarefa tem um Usuario
                .WithMany(u => u.Tarefas)           // Um Usuario pode ter muitas Tarefas
                .HasForeignKey(t => t.usuarioid);   // Chave estrangeira em Tarefa é UsuarioId

            // Configuração da chave primária composta na tabela intermediária
            modelBuilder.Entity<UsuarioPerfilUsuario>()
                .HasKey(up => new { up.UsuarioId, up.IdPerfilUsuario });

            // Configuração do relacionamento muitos-para-muitos
            modelBuilder.Entity<UsuarioPerfilUsuario>()
                .HasOne(up => up.Usuario)
                .WithMany(u => u.UsuariosPerfis)
                .HasForeignKey(up => up.UsuarioId);

            modelBuilder.Entity<UsuarioPerfilUsuario>()
                .HasOne(up => up.PerfilUsuario)
                .WithMany(p => p.UsuariosPerfis)
                .HasForeignKey(up => up.IdPerfilUsuario);
        }
    }
}
