namespace ApiEscapeRank
{
    public partial class SalasCategorias
    {
        public string SalaId { get; set; }
        public string CategoriaId { get; set; }

        public virtual Categoria Categoria { get; set; }
        public virtual Sala Sala { get; set; }
    }
}
