/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace ApiEscapeRank.Modelos
{
    public partial class UsuariosAmigos
    {
        public int UsuarioId { get; set; }
        public int AmigoId { get; set; }
        public Estado Estado { get; set; }

        public virtual Usuario Amigo { get; set; }
        public virtual Usuario Usuario { get; set; }
    }

    public enum Estado
    {
        pendiente,
        aceptado,
        borrado
    }
}
