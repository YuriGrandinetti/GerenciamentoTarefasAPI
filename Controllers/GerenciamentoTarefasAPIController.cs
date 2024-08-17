using Microsoft.AspNetCore.Mvc;
using GerenciamentoTarefasAPI.Models;
using GerenciamentoTarefasAPI.Repository;
using GerenciamentoTarefasAPI.Services;
using System.Threading.Tasks;
using static GerenciamentoTarefas.Domain.Enumeradores;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace GerenciamentoTarefasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefasController : ControllerBase
    {
        private readonly GerenciamentoTarefasContext _context;
        private readonly NotificationService _notificationService;
        private readonly RabbitMQLogger _rabbitMQLogger;

        public TarefasController(GerenciamentoTarefasContext context, RabbitMQService rabbitMQService)
        {
            _context = context;
            _notificationService = new NotificationService(rabbitMQService);
            _rabbitMQLogger = new RabbitMQLogger(rabbitMQService);
        }
              

        [HttpPut("{id}/iniciar")]
        [SwaggerOperation(Summary = "Inicia o progresso de uma tarefa.", Description = "Altera o status de uma tarefa para 'Em Progresso'.")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> IniciarTarefa(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null)
            {
                _rabbitMQLogger.LogError($"Tentativa de iniciar tarefa não encontrada: {id}");
                return NotFound();
            }

            tarefa.Status = StatusTarefa.EmProgresso.ToString();
            await _context.SaveChangesAsync();

            _rabbitMQLogger.LogInformation($"Tarefa iniciada: {tarefa.Descricao}");
            _rabbitMQLogger.LogInformation($"Status da tarefa {tarefa.Descricao} alterado para Em Progresso");

            return NoContent();
        }

        [HttpPut("{id}/concluir")]
        [SwaggerOperation(Summary = "Conclui uma tarefa existente.", Description = "Atualiza o status de uma tarefa existente para 'Concluída' e envia uma notificação de conclusão para o RabbitMQ.")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ConcluirTarefa(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null)
            {
                _rabbitMQLogger.LogError($"Tentativa de concluir tarefa não encontrada: {id}");
                return NotFound();
            }

            tarefa.Status = StatusTarefa.Concluida.ToString();
            await _context.SaveChangesAsync();

            _rabbitMQLogger.LogInformation($"Tarefa concluída: {tarefa.Descricao}");
            _rabbitMQLogger.LogInformation($"Status da tarefa {tarefa.Descricao} alterado para Concluída");
            _notificationService.EnviarNotificacaoDeTarefaConcluida(tarefa.Descricao);

            return NoContent();
        }

        [HttpPut("{id}/cancelar")]
        [SwaggerOperation(Summary = "Cancela uma tarefa existente.", Description = "Atualiza o status de uma tarefa existente para 'Cancelada'.")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CancelarTarefa(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null)
            {
                _rabbitMQLogger.LogError($"Tentativa de cancelar tarefa não encontrada: {id}");
                return NotFound();
            }

            tarefa.Status = StatusTarefa.Cancelada.ToString();
            await _context.SaveChangesAsync();

            _rabbitMQLogger.LogInformation($"Tarefa cancelada: {tarefa.Descricao}");
            _rabbitMQLogger.LogInformation($"Status da tarefa {tarefa.Descricao} alterado para Cancelada");

            return NoContent();
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Obtém todas as tarefas.", Description = "Recupera a lista completa de tarefas do banco de dados.")]
        [ProducesResponseType(typeof(IEnumerable<Tarefa>), 200)]
        public async Task<IActionResult> ObterTarefas()
        {
            var tarefas = await _context.Tarefas.ToListAsync();
            _rabbitMQLogger.LogInformation($"Obtendo todas as tarefas. Total de tarefas: {tarefas.Count}");
            return Ok(tarefas);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtém uma tarefa por ID.", Description = "Recupera uma tarefa específica do banco de dados usando o ID fornecido.")]
        [ProducesResponseType(typeof(Tarefa), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ObterTarefaPorId(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null)
            {
                _rabbitMQLogger.LogError($"Tentativa de obter tarefa não encontrada: {id}");
                return NotFound();
            }

            _rabbitMQLogger.LogInformation($"Obtendo tarefa: {tarefa.Descricao}");
            return Ok(tarefa);
        }

        [HttpGet("pendentes")]
        [SwaggerOperation(Summary = "Obtém todas as tarefas pendentes.", Description = "Recupera a lista completa de tarefas com status 'Pendente' do banco de dados.")]
        [ProducesResponseType(typeof(IEnumerable<Tarefa>), 200)]
        public async Task<IActionResult> ObterTarefasPendentes()
        {
            var tarefasPendentes = await _context.Tarefas
                .Where(t => t.Status == StatusTarefa.Pendente.ToString())
                .ToListAsync();

            _rabbitMQLogger.LogInformation($"Obtendo todas as tarefas pendentes. Total de tarefas pendentes: {tarefasPendentes.Count}");
            return Ok(tarefasPendentes);
        }

        [HttpGet("concluidas")]
        [SwaggerOperation(Summary = "Obtém todas as tarefas concluídas.", Description = "Recupera a lista completa de tarefas com status 'Concluída' do banco de dados.")]
        [ProducesResponseType(typeof(IEnumerable<Tarefa>), 200)]
        public async Task<IActionResult> ObterTarefasConcluidas()
        {
            var tarefasConcluidas = await _context.Tarefas
                .Where(t => t.Status == StatusTarefa.Concluida.ToString())
                .ToListAsync();

            _rabbitMQLogger.LogInformation($"Obtendo todas as tarefas concluídas. Total de tarefas concluídas: {tarefasConcluidas.Count}");
            return Ok(tarefasConcluidas);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cria uma nova tarefa para usuário.", Description = "Adiciona uma nova tarefa ao banco de dados associada ao usuário e envia uma notificação de criação para o RabbitMQ.")]
        [ProducesResponseType(typeof(Tarefa), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CriarTarefa([FromBody] Tarefa novaTarefa)
        {

            // Verifica se o usuário existe
            var usuario = await _context.Usuarios.FindAsync(novaTarefa.usuarioid);
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado");
            }

            // Adiciona a nova tarefa ao contexto
            _context.Tarefas.Add(novaTarefa);
            await _context.SaveChangesAsync();

            // Envia a notificação para o RabbitMQ
            _rabbitMQLogger.LogInformation($"Tarefa criada: {novaTarefa.Descricao} para o usuário {usuario.Nome}");
            _notificationService.EnviarNotificacaoDeTarefaCriada(novaTarefa.Descricao);

            return CreatedAtAction(nameof(ObterTarefaPorId), new { id = novaTarefa.Id }, novaTarefa);
        }



       

    }
}
