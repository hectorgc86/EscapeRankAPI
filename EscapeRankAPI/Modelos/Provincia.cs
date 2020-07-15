using System.Collections.Generic;

/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace EscapeRankAPI.Modelos
{
    public partial class Provincia
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }

        public virtual ICollection<Ciudad> Ciudades { get; set; }

        public Provincia()
        {
            Ciudades = new HashSet<Ciudad>();
        }
    }
}
