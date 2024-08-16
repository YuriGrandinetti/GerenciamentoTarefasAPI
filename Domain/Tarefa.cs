namespace GerenciamentoTarefasAPI.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataVencimento { get; set; }
        public string Status { get; set; }
    }
}