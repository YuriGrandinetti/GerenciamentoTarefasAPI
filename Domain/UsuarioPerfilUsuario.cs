using GerenciamentoTarefasAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GerenciamentoTarefas.Domain
{
    [Table("usuarios_perfilusuario")]
    public class UsuarioPerfilUsuario
    {
        
        [Column("usuarioid")]
        public int UsuarioId { get; set; }
        [JsonIgnore]  // Ignora a desserialização
        public Usuario Usuario { get; set; }
        [Column("idperfilusuario")]
        public int IdPerfilUsuario { get; set; }
        [JsonIgnore]  // Ignora a desserialização
        public PerfilUsuario PerfilUsuario { get; set; }
    }
}
