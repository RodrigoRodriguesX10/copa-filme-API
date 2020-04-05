using CopaFilmes.Dominio.Entidades;
using CopaFilmes.Dominio.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CopaFilmes.API.Controllers
{
    [Route("api/filmes")]
    public class FilmeController : Controller
    {
        private readonly IRepository<Filme> repository;

        public FilmeController(IRepository<Filme> repository)
        {
            this.repository = repository;
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

        }
    }
}
