/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace EscapeRankAPI.Modelos
{
    public partial class Valoracion
    {
        public int Id { get; set; }
        public int? Estrellas { get; set; }
        public string Comentario { get; set; }
        public string SalaId { get; set; }

        public virtual Sala Sala { get; set; }
    }
}
