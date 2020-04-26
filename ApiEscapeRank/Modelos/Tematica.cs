using System.Collections.Generic;

namespace ApiEscapeRank.Modelos
{
    public partial class Tematica
    {
        public string Id { get; set; }
        public string Tipo { get; set; }
        public int NumeroSalas { get; set; }

        public virtual ICollection<SalasTematicas> SalasTematicas { get; set; }

        public Tematica()
        {
            SalasTematicas = new HashSet<SalasTematicas>();
        }
    }
}
