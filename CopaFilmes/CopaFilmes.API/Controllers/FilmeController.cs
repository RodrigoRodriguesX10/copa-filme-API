using CopaFilmes.API.ViewModels;
using CopaFilmes.Dominio.Entidades;
using CopaFilmes.Dominio.Repositorio;
using CopaFilmes.Dominio.Servicos;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CopaFilmes.API.Controllers
{
    [Route("api/filmes")]
    public class FilmeController : Controller
    {
        private readonly IRepository<Filme> repository;
        private readonly ServicoFilme servico;

        public FilmeController(IRepository<Filme> repository, ServicoFilme servico)
        {
            this.repository = repository;
            this.servico = servico;
        }
        [HttpGet("")]
        public IActionResult GetAll()
        {
            try
            {
                var res = repository.Consulta();
                return res != null ? Ok(res) : StatusCode(400, repository.Mensagens);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var res = repository.Retorna(id);
                return res != null ? Ok(res) : StatusCode(404, repository.Mensagens);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("[action]")]
        public IActionResult CriarCompeticao([FromBody] SelecaoViewModel selecao)
        {
            try
            {
                var competicao = servico.GetResultadoCompeticao(selecao?.Filmes);
                return Ok(new { competicao.vencedor, competicao.vice });
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(400, new { Error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return StatusCode(422, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("[action]")] // apenas para visualização do resultado
        public IActionResult MontarFases([FromBody] SelecaoViewModel selecao)
        {
            try
            {
                var chaves = servico.CriarCompeticao(selecao?.Filmes);
                var fases = servico.GetFasesCompeticao(chaves);
                return Ok(fases);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(400, new { Error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return StatusCode(422, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
