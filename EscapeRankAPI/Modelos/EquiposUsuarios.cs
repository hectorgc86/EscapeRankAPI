/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace EscapeRankAPI.Modelos
{
    public partial class EquiposUsuarios
    {
        public int EquipoId { get; set; }
        public int UsuarioId { get; set; }

        public virtual Equipo Equipo { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
