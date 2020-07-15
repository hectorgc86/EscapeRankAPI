using System.Collections.Generic;

/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace EscapeRankAPI.Modelos
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
