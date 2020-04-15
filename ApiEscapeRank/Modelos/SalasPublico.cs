namespace ApiEscapeRank.Modelos
{
    public partial class SalasPublico
    {
        public string SalaId { get; set; }
        public string PublicoId { get; set; }

        public virtual Publico Publico { get; set; }
        public virtual Sala Sala { get; set; }
    }
}
