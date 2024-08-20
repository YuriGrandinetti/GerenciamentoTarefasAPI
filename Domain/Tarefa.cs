using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GerenciamentoTarefas.Domain.Enumeradores;

namespace GerenciamentoTarefasAPI.Models
{
    [Table("tarefas")]
    public class Tarefa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]  // Especifica que o campo "id" no banco de dados corresponde à propriedade "Id"
        public int Id { get; set; }

        [Column("descricao")]  // Especifica que o campo "descricao" no banco de dados corresponde à propriedade "Descricao"
        public string Descricao { get; set; }

        [Column("datavencimento", TypeName = "date")] // Especifica que o campo "datavencimento" no banco de dados corresponde à propriedade "DataVencimento"
        public DateTime DataVencimento { get; set; }

        [Column("status")]  // Especifica que o campo "status" no banco de dados corresponde à propriedade "Status"
        public string Status { get; set; }       
       

        [Column("usuarioid")]
        public Int32 usuarioid { get; set; }


        // Propriedade de navegação para a relação com o usuário
        [ForeignKey("usuarioid")]
        public Usuario Usuario { get; set; }


    }


}