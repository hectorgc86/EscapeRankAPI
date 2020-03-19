using System.Collections.Generic;

namespace ApiEscapeRank
{
    public partial class Dificultad
    {
        public Dificultad()
        {
            Salas = new HashSet<Sala>();
        }

        public string Id { get; set; }
        public string Tipo { get; set; }

        public virtual ICollection<Sala> Salas { get; set; }
    }
}
