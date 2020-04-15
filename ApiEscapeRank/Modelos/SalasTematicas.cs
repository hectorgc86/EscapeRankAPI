namespace ApiEscapeRank.Modelos
{
    public partial class SalasTematicas
    {
        public string SalaId { get; set; }
        public string TematicaId { get; set; }

        public virtual Sala Sala { get; set; }
        public virtual Tematica Tematica { get; set; }
    }
}
