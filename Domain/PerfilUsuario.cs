using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GerenciamentoTarefas.Domain
{
    [Table("perfilusuario")]  // Especifica que o nome da tabela no banco de dados é "perfilusuario"
    public class PerfilUsuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("idperfilusuario")]  
        public int IdPerfilUsuario { get; set; }
        [Column("descricaoperfil")]  // Especifica que o campo "descricaoperfil" no banco de dados corresponde à propriedade "DescricaoPerfil"

        public string? DescricaoPerfil { get; set; }
        [JsonIgnore]  // Ignora a desserialização
        public ICollection<UsuarioPerfilUsuario> UsuariosPerfis { get; set; }
    }
}
