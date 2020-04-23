using System.Collections.Generic;

namespace ApiEscapeRank.Modelos
{
    public partial class Equipo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Avatar { get; set; }

        public virtual ICollection<EquiposUsuarios> EquiposUsuarios { get; set; }
        public virtual ICollection<Partida> Partidas { get; set; }

        public Equipo()
        {
            EquiposUsuarios = new HashSet<EquiposUsuarios>();
            Partidas = new HashSet<Partida>();
        }
    }
}
