using Microsoft.AspNetCore.Mvc;
using GerenciamentoTarefasAPI.Models;
using GerenciamentoTarefasAPI.Services;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using GerenciamentoTarefas.Domain;
using GerenciamentoTarefas.Domain.Interfaces;

namespace GerenciamentoTarefasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

       // private readonly UsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("registrar")]
        [SwaggerOperation(Summary = "Registra um novo usuário.", Description = "Registra um novo usuário com nome, email e senha.")]
        [ProducesResponseType(typeof(Usuario), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RegistrarUsuario(UsuariosCreatedDto novoUsuario)
        {
            var usuario = await _usuarioService.RegistrarUsuario(novoUsuario);
            return CreatedAtAction(nameof(ObterUsuarioPorId), new { id = usuario.Id }, usuario);
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Faz login de um usuário.", Description = "Valida as credenciais do usuário (email e senha) e retorna um token JWT.")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> LoginUsuario([FromBody] LoginRequest loginUsuario)
        {
            var usuario = await _usuarioService.LoginUsuario(loginUsuario.Email, loginUsuario.Senha);

            if (usuario == null)
            {
                return BadRequest("Credenciais inválidas.");
            }

            var token = _usuarioService.GerarToken(usuario);
            return Ok(new { Token = token });
        }


        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtém um usuário por ID.", Description = "Recupera um usuário específico do banco de dados usando o ID fornecido.")]
        [ProducesResponseType(typeof(Usuario), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ObterUsuarioPorId(int id)
        {
            var usuario = await _usuarioService.ObterUsuarioPorId(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpGet("getUsuarios")]
        [SwaggerOperation(Summary = "Carrega todos os usuários.", Description = "Retorna uma lista de todos os usuários.")]
        [ProducesResponseType(typeof(IEnumerable<UauarioDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> getUsuarios()
        {
            var usuarios = await _usuarioService.ObterTodosUsuarios();

            if (usuarios == null || usuarios.Count == 0)
            {
                return NotFound("Nenhum usuário encontrado.");
            }

            return Ok(usuarios);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza um usuário existente.", Description = "Atualiza as informações de um usuário existente com base no ID fornecido.")]
        [ProducesResponseType(typeof(Usuario), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UsuarioAtualizadoDto usuarioAtualizado)
        {
            var usuario = await _usuarioService.UpdateUsuario(id, usuarioAtualizado);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }


    }
}
