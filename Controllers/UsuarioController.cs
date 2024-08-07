using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Usuário.Models;
using Usuário.Repository;

namespace Usuário.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioController(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ususario = await _repository.BuscaUsuarios();

            return ususario.Any() ? Ok(ususario) : NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ususario = await _repository.BuscaUsuario(id);

            return ususario != null ? Ok(ususario) : NotFound("Usuario Não Econtrado");
        }

        [HttpPost]
        public async Task<IActionResult> Post(Usuario usuario)
        {
            _repository.AdicionaUsuario(usuario);

            return await _repository.SaveChangesAsync() ? Ok("Usuário adicionado com sucesso"): BadRequest("Erro ao salvar o usuário");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Usuario usuario){
            var usuarioBanco = await _repository.BuscaUsuario(id);

            if(usuarioBanco == null) return NotFound("Usuário não econtrado");

            usuarioBanco.Name= usuario.Name ?? usuarioBanco.Name;
            usuarioBanco.DataNascimento = usuario.DataNascimento != new DateTime() ? usuario.DataNascimento : usuarioBanco.DataNascimento;

            _repository.AdicionaUsuario(usuarioBanco);

            return await _repository.SaveChangesAsync() ? Ok("Usuário atualizado com sucesso") : BadRequest("Erro ao atualizar o usuário");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id){
            var usuarioBanco = await _repository.BuscaUsuario(id);

            if(usuarioBanco == null) return NotFound("Usuário não econtrado");

            _repository.DeletaUsuario(usuarioBanco);

            return await _repository.SaveChangesAsync() ? Ok("Usuário deletado com sucesso") : BadRequest("Erro ao deletar o usuário");
        }
    }
}