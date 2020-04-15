namespace ApiEscapeRank.Modelos
{
    public partial class Valoracion
    {
        public int Id { get; set; }
        public int? Estrellas { get; set; }
        public string Comentario { get; set; }
        public string SalaId { get; set; }

        public virtual Sala Sala { get; set; }
    }
}
