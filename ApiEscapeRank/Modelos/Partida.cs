namespace ApiEscapeRank.Modelos
{
    public partial class Partida
    {
        public int Id { get; set; }
        public string Fecha { get; set; }
        public string Minutos { get; set; }
        public string Segundos { get; set; }
        public string SalaId { get; set; }
        public int EquipoId { get; set; }

        public virtual Equipo Equipo { get; set; }
        public virtual Sala Sala { get; set; }
    }
}
