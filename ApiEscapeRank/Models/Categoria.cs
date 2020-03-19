using System.Collections.Generic;

namespace ApiEscapeRank
{
    public partial class Categoria
    {
        public Categoria()
        {
            SalasCategorias = new HashSet<SalasCategorias>();
        }

        public string Id { get; set; }
        public string Tipo { get; set; }

        public virtual ICollection<SalasCategorias> SalasCategorias { get; set; }
    }
}
