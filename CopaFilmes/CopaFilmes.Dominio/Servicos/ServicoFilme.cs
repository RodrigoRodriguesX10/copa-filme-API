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
        /// <param name="sequencial">Define se as chaves vao ser geradas sequencialmente (
        /// o 1o contra o 2o, o 3o contra o 4o e assim por diante) ou pelos extremos (o primeiro contra o ultimo,
        /// o segundo contra o penultimo e assim por diante)</param>
        /// <returns>Retorna um array de Chave com Length igual a metade da quantidade de filmes</returns>
        /// <exception cref="ArgumentException">Lança exceção caso a lista
        /// contenha filmes repetidos ou não contenha uma quantidade par de filmes</exception>
        public Chave[] CriarChaves(List<Filme> filmesEscolhidos, bool sequencial = true)
        {
            if (filmesEscolhidos == null)
            {
                throw new ArgumentNullException("A lista de filmes não pode ser nula");
            }
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
            int chave1, chave2;
            for (int i = 0; i < qtdChaves; i++)
            {
                (chave1, chave2) = sequencial ?
                    (i * 2, i * 2 + 1) :
                    (i, quantidade - 1 - i);
                chaves[i] = new Chave(filmesEscolhidos[chave1], filmesEscolhidos[chave2]);
            }
            return chaves;
        }

        public Chave[] CriarCompeticao(string[] filmesEscolhidos)
        {
            if (filmesEscolhidos == null)
            {
                throw new ArgumentNullException("A lista não pode ser nula");
            }
            if (filmesEscolhidos.Length != 8)
            {
                throw new ArgumentException("A lista deve conter uma quantidade par de filmes");
            }
            var ordenados = repository.ConsultaEspecifica(q =>
                from filme in q
                where filmesEscolhidos.Contains(filme.Id)
                orderby filme.Titulo.ToUpper()
                select filme
            );
            if (ordenados.Count != 8)
            {
                throw new ArgumentException("A lista deve conter 8 filmes válidos");
            }
            var chaves = CriarChaves(ordenados, false);
            return chaves;
        }

        public List<Chave[]> GetFasesCompeticao(Chave[] chaves)
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

        public (Filme vencedor, Filme vice) GetResultadoCompeticao(string[] competicao)
        {
            var chaves = CriarCompeticao(competicao);
            var x = GetFasesCompeticao(chaves);
            var ultima = x.Last().First();
            var vencedor = ultima.GetVencedor();
            return (vencedor, vencedor == ultima.Filme1 ? ultima.Filme2 : ultima.Filme1);
        }
    }
}
