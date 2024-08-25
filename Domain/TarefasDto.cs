namespace GerenciamentoTarefas.Domain
{
    public class TarefasDto
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public DateTime DataVencimento { get; set; }

        public string Status { get; set; }

        public Int32 usuarioid { get; set; }
    }
}
