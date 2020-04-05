using CopaFilmes.Dominio.Entidades;

namespace CopaFilmes.Dominio.ViewModels
{
    public class Chave
    {
        public Chave(Filme filme1, Filme filme2) => 
            (Filme1, Filme2) = (filme1, filme2);

        public Filme Filme1 { get; set; }
        public Filme Filme2 { get; set; }

        public string Titulo => Filme1 != null && Filme2 != null ? 
            $"{Filme1.Titulo} x {Filme2.Titulo}" : null;

        public Filme GetVencedor()
        {
            if (Filme1.Nota == Filme2.Nota)
            {
                return Filme1.Titulo.CompareTo(Filme2.Titulo) == -1 ?
                    Filme1 : Filme2;
            }
            return Filme1.Nota > Filme2.Nota ?
                Filme1 : Filme2;
        }
    }
}
