using CopaFilmes.Dominio.Entidades;
using CopaFilmes.Dominio.Repositorio;
using CopaFilmes.Dominio.Servicos;
using CopaFilmes.Dominio.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CopaFilmes.Tests.Servico
{
    [TestFixture]
    public class TesteServicoFilme
    {
        /* Chaves:
         * filme8 X
         * filme1       filme8
         *                 X        filme8
         * filme7 X     filme7
         * filme2
         *                             X         filme8
         * filme6 X     (empate)
         * filme3       filme3
         *                 X        filme4
         * filme5 X     filme4
         * filme4
         */
        Filme Filme1 = new Filme { Nota = 6, Ano = 2000, Titulo = "A filme", Id = "filme1" };
        Filme Filme2 = new Filme { Nota = 4, Ano = 2000, Titulo = "B filme", Id = "filme2" };
        Filme Filme3 = new Filme { Nota = 8, Ano = 2000, Titulo = "C filme", Id = "filme3" };
        Filme Filme4 = new Filme { Nota = 9, Ano = 2000, Titulo = "D Vice vencedor", Id = "filme4" };
        Filme Filme5 = new Filme { Nota = 8, Ano = 2000, Titulo = "E filme", Id = "filme5" };
        Filme Filme6 = new Filme { Nota = 8, Ano = 2000, Titulo = "F filme", Id = "filme6" };
        Filme Filme7 = new Filme { Nota = 7, Ano = 2000, Titulo = "G filme", Id = "filme7" };
        Filme Filme8 = new Filme { Nota = 10, Ano = 2000, Titulo = "O vencedor", Id = "filme8" };
        List<Filme> Selecionados = null;
        private ServicoFilme servicoFilme;
        string[] Ids;
        [OneTimeSetUp]
        public void SetUp()
        {
            Selecionados = new List<Filme>
            {
                // lista desordenada
                Filme8,
                Filme7,
                Filme6,
                Filme5,
                Filme4,
                Filme3,
                Filme2,
                Filme1
            };
            Ids = Selecionados.Select(s => s.Id).ToArray();
            var mockRepositorioFilme = TesteRepositorioFilme.CreateRepositoryMock(Selecionados);
            servicoFilme = new ServicoFilme(mockRepositorioFilme.Object);
        }

        [Test]
        public void DeveCriarChavesSequencial()
        {
            var ordenados = Selecionados.OrderBy(f => f.Titulo.ToUpper()).ToList();
            var chaves = servicoFilme.CriarChaves(ordenados);
            Assert.AreEqual(chaves.Length, 4);
            Assert.AreEqual(chaves[0].Filme1, Filme1);
            Assert.AreEqual(chaves[0].Filme2, Filme2);
            Assert.AreEqual(chaves[1].Filme1, Filme3);
            Assert.AreEqual(chaves[1].Filme2, Filme4);
            Assert.AreEqual(chaves[2].Filme1, Filme5);
            Assert.AreEqual(chaves[2].Filme2, Filme6);
            Assert.AreEqual(chaves[3].Filme1, Filme7);
            Assert.AreEqual(chaves[3].Filme2, Filme8);
        }

        [Test]
        public void DeveCriarChavesNaoSequencial()
        {
            var ordenados = Selecionados.OrderBy(f => f.Titulo.ToUpper()).ToList();
            var chaves = servicoFilme.CriarChaves(ordenados, false);
            Assert.AreEqual(chaves.Length, 4);
            Assert.AreEqual(chaves[0].Filme1, Filme1);
            Assert.AreEqual(chaves[0].Filme2, Filme8);
            Assert.AreEqual(chaves[1].Filme1, Filme2);
            Assert.AreEqual(chaves[1].Filme2, Filme7);
            Assert.AreEqual(chaves[2].Filme1, Filme3);
            Assert.AreEqual(chaves[2].Filme2, Filme6);
            Assert.AreEqual(chaves[3].Filme1, Filme4);
            Assert.AreEqual(chaves[3].Filme2, Filme5);
        }

        [Test]
        public void NaoDeveCriarChaves()
        {
            var impar = Selecionados.ToArray()[1..8];
            Assert.Throws(typeof(ArgumentException),
                () => servicoFilme.CriarChaves(impar.ToList()));

            Assert.Throws(typeof(ArgumentNullException),
                () => servicoFilme.CriarChaves(null));

            var repetidos = Selecionados.ToArray();
            repetidos[0] = repetidos[1];

            Assert.Throws(typeof(ArgumentException),
                () => servicoFilme.CriarChaves(repetidos.ToList()));
        }

        [Test]
        public void DeveRetornarResultadoCompeticao()
        {
            var (vencedor, vice) = servicoFilme.GetResultadoCompeticao(Selecionados.Select(s => s.Id).ToArray());
            Assert.AreEqual(vencedor, Filme8);
            Assert.AreEqual(vice, Filme4);
        }

        [Test]
        public void DeveRetornarVencedorChave()
        {
            var chave = new Chave(Filme3, Filme4);
            var vencedor = chave.GetVencedor();
            Assert.AreEqual(Filme4, vencedor);
            chave = new Chave(Filme4, Filme3);
            vencedor = chave.GetVencedor();
            Assert.AreEqual(Filme4, vencedor);
        }

        [Test]
        public void DeveCriarChavesCompeticao()
        {
            var chaves = servicoFilme.CriarCompeticao(Ids);
            Assert.AreEqual(chaves.Length, 4);
            Assert.AreEqual(chaves[0].Filme1, Filme1);
            Assert.AreEqual(chaves[0].Filme2, Filme8);
            Assert.AreEqual(chaves[1].Filme1, Filme2);
            Assert.AreEqual(chaves[1].Filme2, Filme7);
            Assert.AreEqual(chaves[2].Filme1, Filme3);
            Assert.AreEqual(chaves[2].Filme2, Filme6);
            Assert.AreEqual(chaves[3].Filme1, Filme4);
            Assert.AreEqual(chaves[3].Filme2, Filme5);
        }

        [Test]
        public void DeveDevolverFasesCompeticao()
        {
            var chaves = servicoFilme.CriarCompeticao(Ids);
            Assert.AreEqual(chaves.Length, 4);
            var fases = servicoFilme.GetFasesCompeticao(chaves);
            Assert.AreEqual(fases.Count, 3);
            Assert.AreEqual(fases[0].Length, 4);
            Assert.AreEqual(fases[^1].Length, 1);
            var ultimaChave = fases[^1][0];
            Assert.AreEqual(ultimaChave.GetVencedor(), Filme8);
        }

        [Test]
        public void NaoDeveCriarChavesCompeticao()
        {
            var impar = Selecionados.ToArray()[1..7];
            Assert.Throws(typeof(ArgumentNullException),
                () => servicoFilme.CriarCompeticao(null));
            Assert.Throws(typeof(ArgumentException),
                () => servicoFilme.CriarCompeticao(impar.Select(i => i.Id).ToArray()));
        }


        [Test]
        public void NaoDeveCriarChavesCompeticaoIdsInvalidos()
        {
            Assert.Throws(typeof(ArgumentException),
                () => servicoFilme.CriarCompeticao(new[] {
                    Ids[0],
                    Ids[0],
                    Ids[0],
                    Ids[0],
                    Ids[0],
                    Ids[0],
                    Ids[0],
                    Ids[0]
                }));

            Assert.Throws(typeof(ArgumentException),
                () => servicoFilme.CriarCompeticao(new[] {
                    Ids[0],
                    Ids[1],
                    Ids[2],
                    Ids[3],
                    Ids[4],
                    Ids[5],
                    Ids[6],
                    Ids[6]
                }));

            Assert.Throws(typeof(ArgumentException),
                () => servicoFilme.CriarCompeticao(new[] {
                    Ids[0],
                    Ids[1],
                    Ids[2],
                    Ids[3],
                    Ids[4],
                    Ids[5],
                    Ids[6],
                    "idinexistente"
                }));
        }
    }
}
