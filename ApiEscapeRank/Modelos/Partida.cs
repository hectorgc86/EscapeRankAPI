using System;

namespace ApiEscapeRank.Modelos
{
    public partial class Partida
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public string Minutos { get; set; }
        public string Segundos { get; set; }
        public string Imagen { get; set; }
        public string SalaId { get; set; }
        public int EquipoId { get; set; }

        public virtual Sala Sala { get; set; }
        public virtual Equipo Equipo { get; set; }
    }

    public class PartidaRequest
    {
        public DateTime? Fecha { get; set; }
        public string Minutos { get; set; }
        public string Segundos { get; set; }
        public byte[] Foto { get; set; }
        public Sala Sala { get; set; }
        public Equipo Equipo { get; set; }
    }
}
