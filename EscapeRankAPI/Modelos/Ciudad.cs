using System.Collections.Generic;

/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace EscapeRankAPI.Modelos
{
    public partial class Ciudad
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string CiudadOrigen { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string ProvinciaId { get; set; }

        public virtual Provincia Provincia { get; set; }
        public virtual ICollection<Companyia> Companyias { get; set; }

        public Ciudad()
        {
            Companyias = new HashSet<Companyia>();
        }
    }
}
