namespace GerenciamentoTarefas.Domain
{
    public class TarefaCreateDto
    {
        public string descricao { get; set; }
        public DateTime datavencimento { get; set; }
        public string status { get; set; }
        public int usuarioid { get; set; }
    }
}
