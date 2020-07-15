using System.Collections.Generic;

/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace ApiEscapeRank.Modelos
{
    public partial class Dificultad
    {
        public string Id { get; set; }
        public string Tipo { get; set; }
        public int NumeroSalas { get; set; }

        public virtual ICollection<Sala> Salas { get; set; }

        public Dificultad()
        {
            Salas = new HashSet<Sala>();
        }
    }
}
