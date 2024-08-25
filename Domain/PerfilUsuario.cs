namespace GerenciamentoTarefas.Domain
{
    public class PerfilUsuario
    {
        public int IdPerfilUsuario { get; set; }
        public string DescricaoPerfil { get; set; }

        public ICollection<UsuarioPerfilUsuario> UsuariosPerfis { get; set; }
    }
}
