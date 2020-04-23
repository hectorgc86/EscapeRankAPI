using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEscapeRank.Modelos
{
    public partial class Categoria
    {
        public string Id { get; set; }
        public string Tipo { get; set; }
        public int NumeroSalas { get; set; }

        public virtual ICollection<SalasCategorias> SalasCategorias { get; set; }

        public Categoria()
        {
            SalasCategorias = new HashSet<SalasCategorias>();
        }
    }
}
