using GerenciamentoTarefasAPI.Models;
using GerenciamentoTarefasAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using GerenciamentoTarefas.Domain;
using GerenciamentoTarefas.Domain.Interfaces;

namespace GerenciamentoTarefasAPI.Services
{
    /// <summary>
    /// Classe criada apenas para fazer o registro do usuario no postgree
    /// e fazer o login 
    /// nao foi criada injecao de dependencia para efeito de tempo.
    /// </summary>
    public class UsuarioService : IUsuarioService
    {
        private readonly GerenciamentoTarefasContext _context;
        private readonly IConfiguration _configuration;

        public UsuarioService(GerenciamentoTarefasContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        /// <summary>
        /// Registrar o usuario apenas pelo swagger, pois ele grava o hash da senha no campo senha
        /// </summary>
        /// <param name="novoUsuario"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Usuario> RegistrarUsuario(UsuariosCreatedDto novoUsuario)
        {
            novoUsuario.senha = HashSenha(novoUsuario.senha);

            // Verifica se já existe um usuário com o mesmo e-mail
            var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == novoUsuario.email);

            if (usuarioExistente != null)
            {
                // Retorna um erro ou uma resposta apropriada, ou lance uma exceção
                throw new InvalidOperationException("Já existe um usuário com este e-mail.");
            }
            // Cria uma nova instância de Tarefa a partir do DTO
            var novousuariodto = new Usuario
            {
                Nome = novoUsuario.nome,
                Email = novoUsuario.email,
                Senha = HashSenha(novoUsuario.senha)

            };
            _context.Usuarios.Add(novousuariodto);
            await _context.SaveChangesAsync();

            return novousuariodto;
        }

        public async Task<Usuario> LoginUsuario(string email, string senha)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null || !VerificarSenha(senha, usuario.Senha))
            {
                return null;
            }

            return usuario;
        }

        public string GerarToken(Usuario usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Usuario> ObterUsuarioPorId(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        private string HashSenha(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(senha);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private bool VerificarSenha(string senhaDigitada, string senhaHash)
        {
            var hashDigitado = HashSenha(senhaDigitada);
            return hashDigitado == senhaHash;
        }

        public async Task<List<UauarioDto>> ObterTodosUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync(); // Aqui, 'Usuarios' deve ser um DbSet<Usuario>

            // Converte a lista de Usuario para UsuarioDto
            var usuariosDto = new List<UauarioDto>();

            foreach (var usuario in usuarios)
            {
                usuariosDto.Add(new UauarioDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                });
            }

            return usuariosDto;
        }

        /// <summary>
        /// Atualiza um usuário existente com os novos dados fornecidos.
        /// </summary>
        /// <param name="id">O ID do usuário a ser atualizado.</param>
        /// <param name="usuarioAtualizado">Objeto DTO contendo os dados atualizados do usuário.</param>
        /// <returns>O usuário atualizado ou null se o usuário não for encontrado.</returns>
        public async Task<Usuario> UpdateUsuario(int id, UsuarioAtualizadoDto usuarioAtualizado)
        {
            // Busca o usuário pelo ID
            var usuarioExistente = await _context.Usuarios.FindAsync(id);

            // Verifica se o usuário existe
            if (usuarioExistente == null)
            {
                throw new InvalidOperationException("Usuário não encontrado.");
            }

            // Atualiza os campos necessários
            usuarioExistente.Nome = usuarioAtualizado.Nome ?? usuarioExistente.Nome;
            usuarioExistente.Email = usuarioAtualizado.Email ?? usuarioExistente.Email;

            //// Atualiza a senha somente se uma nova senha foi fornecida
            //if (!string.IsNullOrEmpty(usuarioAtualizado.Senha))
            //{
            //    usuarioExistente.Senha = HashSenha(usuarioAtualizado.Senha);
            //}

            // Salva as alterações no banco de dados
            await _context.SaveChangesAsync();

            return usuarioExistente;
        }
    }
}

