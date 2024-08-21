using Microsoft.AspNetCore.Mvc;
using GerenciamentoTarefasAPI.Models;
using GerenciamentoTarefasAPI.Services;
using System.Threading.Tasks;
using static GerenciamentoTarefas.Domain.Enumeradores;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using GerenciamentoTarefas.Domain;
using System.Linq;
using GerenciamentoTarefas.Domain.Interfaces;
using GerenciamentoTarefasAPI.Repository;

namespace GerenciamentoTarefasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefasController : ControllerBase
    {
        private readonly GerenciamentoTarefasContext _context;
        private readonly ITarefasRepository _tarefasRepository;
        private readonly NotificationService _notificationService;
        private readonly RabbitMQLogger _rabbitMQLogger;

        public TarefasController(GerenciamentoTarefasContext context,
            ITarefasRepository tarefasRepository,
            RabbitMQService rabbitMQService)
        {
            _context = context;
            _tarefasRepository = tarefasRepository;
            _notificationService = new NotificationService(rabbitMQService);
            _rabbitMQLogger = new RabbitMQLogger(rabbitMQService);
        }

        // Mantendo os métodos existentes, mas substituindo o uso do contexto direto pelo repositório.

        [HttpPut("{id}/iniciar")]
        [SwaggerOperation(Summary = "Inicia o progresso de uma tarefa.", Description = "Altera o status de uma tarefa para 'Em Progresso'.")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> IniciarTarefa(int id)
        {
            var tarefa = await _tarefasRepository.ObterTarefaPorId(id);
            if (tarefa == null)
            {
                _rabbitMQLogger.LogError($"Tentativa de iniciar tarefa não encontrada: {id}");
                return NotFound();
            }

            tarefa.Status = StatusTarefa.EmProgresso.ToString();
            await _tarefasRepository.AtualizarTarefa(tarefa);

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
            var tarefa = await _tarefasRepository.ObterTarefaPorId(id);
            if (tarefa == null)
            {
                _rabbitMQLogger.LogError($"Tentativa de concluir tarefa não encontrada: {id}");
                return NotFound();
            }

            tarefa.Status = StatusTarefa.Concluida.ToString();
            await _tarefasRepository.AtualizarTarefa(tarefa);

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
            var tarefa = await _tarefasRepository.ObterTarefaPorId(id);
            if (tarefa == null)
            {
                _rabbitMQLogger.LogError($"Tentativa de cancelar tarefa não encontrada: {id}");
                return NotFound();
            }

            tarefa.Status = StatusTarefa.Cancelada.ToString();
            await _tarefasRepository.AtualizarTarefa(tarefa);

            _rabbitMQLogger.LogInformation($"Tarefa cancelada: {tarefa.Descricao}");
            _rabbitMQLogger.LogInformation($"Status da tarefa {tarefa.Descricao} alterado para Cancelada");

            return NoContent();
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Obtém todas as tarefas.", Description = "Recupera a lista completa de tarefas do banco de dados.")]
        [ProducesResponseType(typeof(IEnumerable<Tarefa>), 200)]
        public async Task<IActionResult> ObterTarefas()
        {
            var tarefas = await _tarefasRepository.ObterTarefas();
            _rabbitMQLogger.LogInformation($"Obtendo todas as tarefas. Total de tarefas: {tarefas.Count()}");
            return Ok(tarefas);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtém uma tarefa por ID.", Description = "Recupera uma tarefa específica do banco de dados usando o ID fornecido.")]
        [ProducesResponseType(typeof(Tarefa), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ObterTarefaPorId(int id)
        {
            var tarefa = await _tarefasRepository.ObterTarefaPorId(id);
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
            var tarefasPendentes = await _tarefasRepository.ObterTarefasPendentes();
            _rabbitMQLogger.LogInformation($"Obtendo todas as tarefas pendentes. Total de tarefas: {tarefasPendentes.Count()}");
            return Ok(tarefasPendentes);
        }

        [HttpGet("concluidas")]
        [SwaggerOperation(Summary = "Obtém todas as tarefas concluídas.", Description = "Recupera a lista completa de tarefas com status 'Concluída' do banco de dados.")]
        [ProducesResponseType(typeof(IEnumerable<Tarefa>), 200)]
        public async Task<IActionResult> ObterTarefasConcluidas()
        {
            var tarefasConcluidas = await _tarefasRepository.ObterTarefasConcluidas();
            _rabbitMQLogger.LogInformation($"Obtendo todas as tarefas concluídas. Total de tarefas: {tarefasConcluidas.Count()}");
            return Ok(tarefasConcluidas);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cria uma nova tarefa para usuário.", Description = "Adiciona uma nova tarefa ao banco de dados associada ao usuário e envia uma notificação de criação para o RabbitMQ.")]
        [ProducesResponseType(typeof(Tarefa), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CriarTarefa([FromBody] TarefaCreateDto novaTarefaDto)
        {
            try
            {
                // Verifica se o usuário existe
                var usuario = await _tarefasRepository.ObterTarefasPorUsuarioId(novaTarefaDto.usuarioid);
                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado");
                }

                // Cria uma nova instância de Tarefa a partir do DTO
                var novaTarefa = new Tarefa
                {
                    Descricao = novaTarefaDto.descricao,
                    DataVencimento = novaTarefaDto.datavencimento,
                    Status = novaTarefaDto.status,
                    usuarioid = novaTarefaDto.usuarioid
                };

                // Adiciona a nova tarefa ao repositório
                await _tarefasRepository.AdicionarTarefa(novaTarefa);

                // Envia a notificação para o RabbitMQ
                _rabbitMQLogger.LogInformation($"Tarefa criada: {novaTarefa.Descricao} para o usuário {novaTarefa.usuarioid}");
                _notificationService.EnviarNotificacaoDeTarefaCriada(novaTarefa.Descricao);

                return CreatedAtAction(nameof(ObterTarefaPorId), new { id = novaTarefa.Id }, novaTarefa);
            }
            catch (Exception e)
            {
                _rabbitMQLogger.LogError($"Erro ao criar tarefa: {e.Message}");
                return StatusCode(500, "Ocorreu um erro ao criar a tarefa.");
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Altera uma tarefa existente.", Description = "Atualiza as informações de uma tarefa existente no banco de dados.")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AlterarTarefa(int id, [FromBody] TarefaUpdateDto tarefaAlteradaDto)
        {
            try
            {
                // Busca a tarefa existente no repositório
                var tarefaExistente = await _tarefasRepository.ObterTarefaPorId(id);
                if (tarefaExistente == null)
                {
                    _rabbitMQLogger.LogError($"Tentativa de alterar tarefa não encontrada: {id}");
                    return NotFound();
                }

                // Atualiza as propriedades da tarefa existente
                tarefaExistente.Descricao = tarefaAlteradaDto.descricao;
                tarefaExistente.Status = tarefaAlteradaDto.status;
                tarefaExistente.DataVencimento = tarefaAlteradaDto.datavencimento;

                // Salva as alterações no repositório
                await _tarefasRepository.AtualizarTarefa(tarefaExistente);

                _rabbitMQLogger.LogInformation($"Tarefa alterada: {tarefaExistente.Descricao}");
                return NoContent();
            }
            catch (Exception e)
            {
                _rabbitMQLogger.LogError($"Erro ao alterar tarefa: {e.Message}");
                return StatusCode(500, "Ocorreu um erro ao alterar a tarefa.");
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove uma tarefa existente.", Description = "Remove uma tarefa do banco de dados pelo ID.")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoverTarefa(int id)
        {
            var tarefaExistente = await _tarefasRepository.ObterTarefaPorId(id);
            if (tarefaExistente == null)
            {
                _rabbitMQLogger.LogError($"Tentativa de remover tarefa não encontrada: {id}");
                return NotFound();
            }

            await _tarefasRepository.RemoverTarefa(tarefaExistente);

            _rabbitMQLogger.LogInformation($"Tarefa removida: {tarefaExistente.Descricao}");
            return NoContent();
        }

        [HttpGet("minhas-tarefas")]
        [SwaggerOperation(Summary = "Obtém as tarefas do usuário logado.", Description = "Recupera a lista de tarefas associadas ao usuário atualmente logado.")]
        [ProducesResponseType(typeof(IEnumerable<Tarefa>), 200)]
        public async Task<IActionResult> ObterMinhasTarefas()
        {

            var allClaims = User.Claims.ToList();
            string valorusuario = null;

            // Loga todos os claims para depuração
            foreach (var claim in allClaims)
            {
                // _rabbitMQLogger.LogInformation($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                valorusuario = claim.Value;
                break;
            }
            var usuarioId = valorusuario; // User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;


            if (string.IsNullOrEmpty(usuarioId))
            {
                _rabbitMQLogger.LogError("Usuário não identificado.");
                return Unauthorized("Usuário não autenticado.");
            }

            // Converte o usuarioId para int (se necessário)
            int usuarioIdInt = int.Parse(usuarioId);

            // Obter as tarefas do usuário logado
            var minhasTarefas = await _tarefasRepository.ObterTarefasPorUsuario(usuarioIdInt);

            _rabbitMQLogger.LogInformation($"Obtendo tarefas do usuário {usuarioIdInt}. Total de tarefas: {minhasTarefas.Count()}");
            return Ok(minhasTarefas);
        }

        [HttpGet("pesquisar")]
        [SwaggerOperation(Summary = "Pesquisa tarefas por descrição, data ou status.", Description = "Pesquisa tarefas com base nos critérios de descrição, data ou status, e retorna as tarefas juntamente com o nome do usuário a que estão vinculadas.")]
        [ProducesResponseType(typeof(IEnumerable<object>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PesquisarTarefas([FromQuery] string? descricao = null, [FromQuery] DateTime? data = null, [FromQuery] string? status = null)
        {
            try
            {
                var query = _context.Tarefas.AsQueryable();

                if (!string.IsNullOrEmpty(descricao))
                {
                    query = query.Where(t => t.Descricao.Contains(descricao));
                }

                if (data.HasValue)
                {
                    query = query.Where(t => t.DataVencimento.Date == data.Value.Date);
                }

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(t => t.Status.ToLower() == status.ToLower());
                }

                var tarefasComUsuarios = await query
                    .Include(t => t.Usuario)
                    .Select(t => new
                    {
                        t.Id,
                        t.Descricao,
                        t.Status,
                        t.DataVencimento,
                        UsuarioNome = t.Usuario.Nome
                    })
                    .ToListAsync();

                if (tarefasComUsuarios.Count == 0)
                {
                    return NotFound("Nenhuma tarefa encontrada com os critérios fornecidos.");
                }

                return Ok(tarefasComUsuarios);
            }
            catch (Exception e)
            {
                _rabbitMQLogger.LogError($"Erro ao pesquisar tarefas: {e.Message}");
                return StatusCode(500, "Ocorreu um erro ao pesquisar as tarefas.");
            }
        }


    }
}
