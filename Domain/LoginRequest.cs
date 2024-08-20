using System.ComponentModel.DataAnnotations;

namespace GerenciamentoTarefas.Domain
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}
