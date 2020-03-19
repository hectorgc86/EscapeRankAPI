using System.Collections.Generic;

namespace ApiEscapeRank
{
    public partial class Tematica
    {
        public Tematica()
        {
            SalasTematicas = new HashSet<SalasTematicas>();
        }

        public string Id { get; set; }
        public string Tipo { get; set; }

        public virtual ICollection<SalasTematicas> SalasTematicas { get; set; }
    }
}
