﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciamentoTarefasAPI.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]  // Especifica que o campo "id" no banco de dados corresponde à propriedade "Id"
        public int Id { get; set; }
        [Column("nome")]  // Especifica que o campo "nome" no banco de dados corresponde à propriedade "Nome"
        public string Nome { get; set; }
        [Column("email")]  // Especifica que o campo "email" no banco de dados corresponde à propriedade "Email"
        public string Email { get; set; }
        [Column("senha")]  // Especifica que o campo "senha" no banco de dados corresponde à propriedade "Senha"
        public string Senha { get; set; }
    }
}
