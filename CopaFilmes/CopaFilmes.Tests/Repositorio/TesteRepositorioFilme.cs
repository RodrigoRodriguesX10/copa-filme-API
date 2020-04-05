using CopaFilmes.Dominio.Entidades;
using CopaFilmes.Dominio.Repositorio;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Where =
    System.Linq.Expressions.Expression<
        System.Func<CopaFilmes.Dominio.Entidades.Filme, bool>>;
using WhereAction =
    System.Func<
        System.Linq.IQueryable<CopaFilmes.Dominio.Entidades.Filme>,
        System.Linq.IQueryable<CopaFilmes.Dominio.Entidades.Filme>>;

namespace CopaFilmes.Tests
{
    [TestFixture]
    public class TesteRepositorioFilme
    {
        public Mock<IRepository<Filme>> MockRepositorioFilme { get; private set; }

        private List<Filme> lista;
        private IRepository<Filme> Repositorio;

        [OneTimeSetUp]
        public void Setup()
        {
            lista = new List<Filme>();
            var ano = 1990;
            for (int i = 1; i <= 100; i++)
            {
                var filme = new Filme
                {
                    Ano = ano + (i % 20),
                    Id = $"filme{i}",
                    Titulo = $"{i.ToString("D3")} Filme",
                    Nota = (double)i / 10
                };
                lista.Add(filme);
            }
            MockRepositorioFilme = new Mock<IRepository<Filme>>();
            MockRepositorioFilme.Setup(x => x.Consulta()).Returns(lista);
            Repositorio = MockRepositorioFilme.Object;

            var query = lista.AsQueryable();

            MockRepositorioFilme.Setup(x => x.ConsultaEspecifica(It.IsAny<WhereAction>()))
                .Returns<WhereAction>(x => x(query).ToList());

            MockRepositorioFilme.Setup(x => x.ConsultaEspecifica(It.IsAny<WhereAction>()))
                .Returns<WhereAction>(x => x(query).ToList());

            MockRepositorioFilme.Setup(x => x.Retorna(It.IsAny<string>()))
                .Returns<string>(x => lista.FirstOrDefault(i => i.Id == x));
        }

        [Test]
        public void TestConsulta()
        {
            var dados = Repositorio.Consulta();
            Assert.IsNotNull(dados);
            Assert.AreEqual(dados.Count, 100);
        }

        [Test]
        public void TestConsultaWhere()
        {
            var dados = Repositorio.Consulta();

            MockRepositorioFilme.Setup(x => x.Consulta(It.IsAny<Where>()))
                .Returns<Where>(predicate => dados.Where(predicate.Compile()).ToList());

            var retorno = Repositorio.Consulta(f => f.Nota > 9);
            Assert.IsNotNull(retorno);
            Assert.AreEqual(retorno.Count, 10);
        }

        [Test]
        public void TestConsultaEspecifica()
        {
            var query = Repositorio.Consulta().AsQueryable();

            var retorno = Repositorio.ConsultaEspecifica(query =>
                    from filme in query
                    where filme.Nota >= 9
                    orderby filme.Nota descending
                    select filme);

            Assert.AreEqual(retorno.Count, 11);
            Assert.AreEqual(retorno.First(), lista.Last());
        }

        [Test]
        public void DeveRetornarRegistroPorId()
        {
            var registro = Repositorio.Retorna("filme1");

            Assert.IsNotNull(registro);
            Assert.AreEqual(registro, lista.First());
        }

        [Test]
        public void NaoDeveRetornarRegistroPorId()
        {
            var registro = Repositorio.Retorna("naoexisteid");
            Assert.IsNull(registro);
        }
    }
}