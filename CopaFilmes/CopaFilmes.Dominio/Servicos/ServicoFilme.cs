using CopaFilmes.Dominio.Entidades;
using CopaFilmes.Dominio.Repositorio;
using CopaFilmes.Dominio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CopaFilmes.Dominio.Servicos
{
    public class ServicoFilme
    {
        private readonly IRepository<Filme> repository;

        public ServicoFilme(IRepository<Filme> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Monta a chaves baseadas em pares de filmes escolhidos
        /// </summary>
        /// <param name="filmesEscolhidos">Pares de filmes escolhidos</param>
        /// <returns>Retorna um array de Chave com Length igual a metade da quantidade de filmes</returns>
        /// <exception cref="ArgumentException">Lança exceção caso a lista
        /// contenha filmes repetidos ou não contenha uma quantidade par de filmes</exception>
        public Chave[] CriarChaves(List<Filme> filmesEscolhidos)
        {
            var quantidade = filmesEscolhidos.Count;
            if (quantidade % 2 != 0)
            {
                throw new ArgumentException("A lista deve conter uma quantidade par de filmes");
            }
            var ids = filmesEscolhidos.Select(f => f.Id).Distinct().ToList();
            if (ids.Count != quantidade)
            {
                throw new ArgumentException("Você deve selecionar 8 filmes diferentes");
            }
            var qtdChaves = quantidade / 2;
            var chaves = new Chave[qtdChaves];
            for (int i = 0; i < qtdChaves; i++)
            {
                chaves[i] = new Chave(filmesEscolhidos[i], filmesEscolhidos[quantidade - 1 - i]);
            }
            return chaves;
        }

        public Chave[] CriarCompeticao(List<Filme> filmesEscolhidos)
        {
            if (filmesEscolhidos.Count != 8)
            {
                throw new ArgumentException("A lista deve conter uma quantidade par de filmes");
            }
            var ordenados = filmesEscolhidos.OrderBy(f => f.Titulo).ToList();
            var chaves = CriarChaves(ordenados);
            return chaves;
        }

        public Chave GetChaveVencedores(Chave chave1, Chave chave2)
        {
            var listaVencedores = new List<Filme>
            {
                chave1.GetVencedor(),
                chave2.GetVencedor()
            };
            return CriarChaves(listaVencedores).First();
        }

        public List<Chave[]> GetChavesCompeticao(Chave[] chaves)
        {
            var res = new List<Chave[]>();
            void PercorreChaveRecursivo(Chave[] fase)
            {
                if (fase.Length == 1)
                {
                    res.Add(fase);
                    return;
                }
                res.Add(fase);
                var novaFase = fase.Select(f => f.GetVencedor()).ToList();
                var c = CriarChaves(novaFase);
                PercorreChaveRecursivo(c);
            }
            PercorreChaveRecursivo(chaves);
            return res;
        }

        public (Filme vencedor, Filme vice) GetResultadoCompeticao(List<Filme> competicao)
        {
            var chaves = CriarCompeticao(competicao);
            var x = GetChavesCompeticao(chaves);
            var ultima = x.Last().First();
            var vencedor = ultima.GetVencedor();
            return (vencedor, vencedor == ultima.Filme1 ? ultima.Filme2 : ultima.Filme1);
        }
    }
}
