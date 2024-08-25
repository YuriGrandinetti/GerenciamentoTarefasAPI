using GerenciamentoTarefas.Domain;
using GerenciamentoTarefas.Domain.Interfaces;
using GerenciamentoTarefas.Services;
using GerenciamentoTarefasAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoTarefas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerfilUsuarioController : ControllerBase
    {
        private readonly IPerfilUsuarioRepository _perfilUsuarioRepository;
        private readonly IUsuarioPerfilUsuarioRepository _usuarioPerfilUsuarioRepository;
        private readonly GerenciamentoTarefasContext _context;
        private readonly NotificationService _notificationService;
        private readonly RabbitMQLogger _rabbitMQLogger;

        public PerfilUsuarioController(
            IPerfilUsuarioRepository perfilUsuarioRepository,
            IUsuarioPerfilUsuarioRepository usuarioPerfilUsuarioRepository,
            GerenciamentoTarefasContext context,
            NotificationService notificationService,
            RabbitMQLogger rabbitMQLogger)
        {
            _perfilUsuarioRepository = perfilUsuarioRepository;
            _usuarioPerfilUsuarioRepository = usuarioPerfilUsuarioRepository;
            _context = context;
            _notificationService = notificationService;
            _rabbitMQLogger = rabbitMQLogger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPerfisUsuarios()
        {
            var perfis = await _perfilUsuarioRepository.GetPerfisUsuariosAsync();
            return Ok(perfis);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerfilUsuarioById(int id)
        {
            var perfil = await _perfilUsuarioRepository.GetPerfilUsuarioByIdAsync(id);
            if (perfil == null) return NotFound();

            return Ok(perfil);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerfilUsuario([FromBody] PerfilUsuario perfilUsuario)
        {
            // Criar o perfil de usuário
            var novoPerfil = await _perfilUsuarioRepository.CreatePerfilUsuarioAsync(perfilUsuario);

            // Registrar a criação no RabbitMQ
            _rabbitMQLogger.LogError($"Perfil de usuário criado: {novoPerfil.DescricaoPerfil}");

            // Enviar notificação sobre a criação do novo perfil
            _notificationService.EnviarNotificacao($"Novo perfil de usuário criado: {novoPerfil.DescricaoPerfil}");

            // Retornar a resposta com o novo perfil criado
            return CreatedAtAction(nameof(GetPerfilUsuarioById), new { id = novoPerfil.IdPerfilUsuario }, novoPerfil);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerfilUsuario(int id)
        {
            // Verificar se o perfil existe
            var perfil = await _perfilUsuarioRepository.GetPerfilUsuarioByIdAsync(id);
            if (perfil == null) return NotFound();

            // Remover o perfil do usuário
            var result = await _perfilUsuarioRepository.DeletePerfilUsuarioAsync(id);

            if (result)
            {
                // Registrar a exclusão no RabbitMQ
                _rabbitMQLogger.LogError($"Perfil de usuário excluído: {perfil.DescricaoPerfil}");

                // Enviar notificação sobre a exclusão do perfil
                _notificationService.EnviarNotificacao($"Perfil de usuário excluído: {perfil.DescricaoPerfil}");

                return NoContent();
            }

            return BadRequest("Erro ao excluir o perfil de usuário.");
        }

        [HttpPost("{usuarioId}/perfis/{perfilUsuarioId}")]
        public async Task<IActionResult> AddPerfilToUsuario(int usuarioId, int perfilUsuarioId)
        {
            await _usuarioPerfilUsuarioRepository.AddPerfilToUsuarioAsync(usuarioId, perfilUsuarioId);

            // Registrar a adição do perfil ao usuário no RabbitMQ
            _rabbitMQLogger.LogError($"Perfil {perfilUsuarioId} adicionado ao usuário {usuarioId}");

            // Enviar notificação sobre a adição do perfil ao usuário
            _notificationService.EnviarNotificacao($"Perfil {perfilUsuarioId} adicionado ao usuário {usuarioId}");

            return Ok();
        }

        [HttpDelete("{usuarioId}/perfis/{perfilUsuarioId}")]
        public async Task<IActionResult> RemovePerfilFromUsuario(int usuarioId, int perfilUsuarioId)
        {
            await _usuarioPerfilUsuarioRepository.RemovePerfilFromUsuarioAsync(usuarioId, perfilUsuarioId);

            // Registrar a remoção do perfil do usuário no RabbitMQ
            _rabbitMQLogger.LogError($"Perfil {perfilUsuarioId} removido do usuário {usuarioId}");

            // Enviar notificação sobre a remoção do perfil do usuário
            _notificationService.EnviarNotificacao($"Perfil {perfilUsuarioId} removido do usuário {usuarioId}");

            return Ok();
        }
    }
}