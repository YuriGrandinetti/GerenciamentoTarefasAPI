namespace GerenciamentoTarefas.Domain
{
    public class TarefaUpdateDto
    {
        public string descricao { get; set; }
        public DateTime datavencimento { get; set; }
        public string status { get; set; }
        public int usuarioid { get; set; }  // Referência ao usuário, mas sem o objeto Usuario completo
    }
}
