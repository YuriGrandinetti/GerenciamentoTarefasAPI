using GerenciamentoTarefasAPI.Models;

namespace GerenciamentoTarefas.Domain
{
    public class UsuarioPerfilUsuario
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int IdPerfilUsuario { get; set; }
        public PerfilUsuario PerfilUsuario { get; set; }
    }
}
