/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace EscapeRankAPI.Modelos
{
    public partial class SalasTematicas
    {
        public string SalaId { get; set; }
        public string TematicaId { get; set; }

        public virtual Sala Sala { get; set; }
        public virtual Tematica Tematica { get; set; }
    }
}
