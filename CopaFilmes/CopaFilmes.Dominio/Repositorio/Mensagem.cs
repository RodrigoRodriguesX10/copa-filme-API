namespace CopaFilmes.Dominio.Repositorio
{
    /// <summary>
    /// Classe que representa uma mensagem de erro do repositório
    /// </summary>
    public class Mensagem
    {
        public Mensagem(string razao, string detalhes) =>
            (Razao, Detalhes) = (razao, detalhes);
        public string Razao { get; set; }
        public string Detalhes { get; set; }
    }
}
