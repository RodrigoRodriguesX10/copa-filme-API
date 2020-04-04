using CopaFilmes.Dominio.Entidades;
using CopaFilmes.Dominio.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text.Json;

namespace RepositorioAPI
{
    public class RepositorioAPI<T> : IRepository<T> where T: Entidade
    {
        private readonly string sourceUrl;
        private readonly string path;

        public RepositorioAPI(string sourceUrl)
        {
            this.sourceUrl = sourceUrl;
            this.path = $"/api/{typeof(T).Name}s";
            Mensagens = new List<Mensagem>();
        }

        public List<Mensagem> Mensagens { get; }

        public List<T> Consulta()
        {
            try
            {
                using var client = new HttpClient();
                var response = client.GetAsync(sourceUrl + path).GetAwaiter().GetResult();
                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!response.IsSuccessStatusCode)
                {
                    Mensagens.Add(new Mensagem(response.StatusCode.ToString(), $"{response.ReasonPhrase}. Details: {json}"));
                    return null;
                }
                var lista = JsonSerializer.Deserialize<List<T>>(json);
                return lista;
            }
            catch(HttpRequestException httpEx)
            {
                Mensagens.Add(new Mensagem("RequestError", httpEx.Message));
            }
            catch (Exception ex)
            {
                Mensagens.Add(new Mensagem("Exception", ex.Message));
            }
            return null;
        }

        public List<T> Consulta(Expression<Func<T, bool>> predicate)
        {
            return Consulta()?.Where(predicate.Compile()).ToList();
        }

        public List<T> ConsultaEspecifica(Func<IQueryable<T>, IQueryable<T>> applyQuery)
        {
            var list = Consulta()?.AsQueryable();
            if (list == null) 
                return null;
            var queryResult = applyQuery(list);
            return queryResult?.ToList();
        }
    }
}
