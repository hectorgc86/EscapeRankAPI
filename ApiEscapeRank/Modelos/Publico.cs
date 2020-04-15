using System.Collections.Generic;

namespace ApiEscapeRank.Modelos
{
    public partial class Publico
    {
        public Publico()
        {
            SalasPublico = new HashSet<SalasPublico>();
        }

        public string Id { get; set; }
        public string Tipo { get; set; }

        public virtual ICollection<SalasPublico> SalasPublico { get; set; }
    }
}
