using CopaFilmes.Dominio.Entidades;
using CopaFilmes.Dominio.Repositorio;
using Moq;
using NUnit.Framework;
using RepositorioAPI;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public class TesteRepositorioApiFilme
    {
        public Mock<RepositorioAPI<Filme>> MockRepositorioFilme { get; private set; }
        private IRepository<Filme> Repositorio;

        [SetUp]
        public void Setup()
        {
            MockRepositorioFilme = new Mock<RepositorioAPI<Filme>>("www.teste.com.br");
            Repositorio = MockRepositorioFilme.Object;

            MockRepositorioFilme.Setup(x => x.GetRequestResult()).Returns(() =>
            {
                Repositorio.Mensagens.Add(new Mensagem("Erro", "Retornando null por default"));
                return null;
            });
        }

        [Test]
        public void TestFalhaConsulta()
        {
            var dados = Repositorio.Consulta();
            Assert.IsNull(dados);
            Assert.IsNotEmpty(Repositorio.Mensagens);
            Assert.AreEqual(Repositorio.Mensagens.First().Razao, "Erro");
        }

        [Test]
        public void TestFalhaGenericaConsulta()
        {
            MockRepositorioFilme.Setup(x => x.GetRequestResult()).Throws(new HttpRequestException("Falhou"));
            var dados = Repositorio.Consulta();
            Assert.IsNull(dados);
            Assert.IsNotEmpty(Repositorio.Mensagens);
            Assert.AreEqual(Repositorio.Mensagens.Count, 1);
            Assert.AreEqual(Repositorio.Mensagens.First().Razao, "RequestError");
        }

        [Test]
        public void TestFalhaHttpConsulta()
        {
            MockRepositorioFilme.Setup(x => x.GetRequestResult()).Throws(new System.Exception("Falhou"));
            var dados = Repositorio.Consulta();
            Assert.IsNull(dados);
            Assert.IsNotEmpty(Repositorio.Mensagens);
            Assert.AreEqual(Repositorio.Mensagens.Count, 1);
            Assert.AreEqual(Repositorio.Mensagens.First().Razao, "Exception");
        }
    }
}