/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace ApiEscapeRank.Modelos
{
    public partial class SalasPublico
    {
        public string SalaId { get; set; }
        public string PublicoId { get; set; }

        public virtual Publico Publico { get; set; }
        public virtual Sala Sala { get; set; }
    }
}
