namespace CopaFilmes.Dominio.Repositorio
{
    /// <summary>
    /// Representa o endereço de um provedor de dados, seja uma connection string
    /// para banco de dados ou uma URL para consumir uma API
    /// </summary>
    public sealed class SourceAddress
    {
        private readonly string sourceUrl;

        public SourceAddress(string sourceUrl)
        {
            this.sourceUrl = sourceUrl;
        }

        public string GetSourceString() => sourceUrl;

        public static implicit operator string(SourceAddress sa) => sa.GetSourceString();

        public static implicit operator SourceAddress(string sa) => new SourceAddress(sa);
    }
}
