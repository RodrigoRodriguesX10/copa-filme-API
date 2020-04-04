using CopaFilmes.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CopaFilmes.Dominio.Repositorio
{
    /// <summary>
    /// Define um repositório para consulta que pode ser um provedor
    /// de dados externo ou um banco de dados comum.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade em que a consulta irá se aplicar</typeparam>
    public interface IRepository<T> where T: Entidade
    {
        List<Mensagem> Mensagens { get; }
        /// <summary>
        /// Consulta todos os itens do tipo T no repositório especificado
        /// </summary>
        /// <returns>Retorna um List contendo todos os itens retornados</returns>
        List<T> Consulta();
        /// <summary>
        /// Consulta todos os itens do tipo T no repositório especificado
        /// sob um predicado passado por parâmetro.
        /// </summary>
        /// <param name="predicate">Expressão que define o predicado que será aplicado sobre a consulta</param>
        /// <returns>Retorna um List contendo todos os itens filtrados.</returns>
        List<T> Consulta(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// Consulta no repositório todos os itens e permite aplicar restrições 
        /// e predicados a essa consulta através de uma função passada por parâmetro
        /// </summary>
        /// <param name="applyQuery">Função que recebe como parâmetro um IQueryable do tipo T
        /// e deve retornar outro IQueryable do tipo T</param>
        /// <returns>Retorna o resultado do IQueryable retornado na função applyQuery chamando o método ToList()</returns>
        List<T> ConsultaEspecifica(Func<IQueryable<T>, IQueryable<T>> applyQuery);
    }
}
