/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace EscapeRankAPI.Modelos
{
    public partial class SalasCategorias
    {
        public string SalaId { get; set; }
        public string CategoriaId { get; set; }

        public virtual Categoria Categoria { get; set; }
        public virtual Sala Sala { get; set; }
    }
}
