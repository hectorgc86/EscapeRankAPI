using System;

/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace ApiEscapeRank.Modelos
{
    public partial class Perfil
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime? Nacido { get; set; }
        public string Telefono { get; set; }
        public string Avatar { get; set; }
        public int? NumeroPartidas { get; set; }
        public int? PartidasGanadas { get; set; }
        public int? PartidasPerdidas { get; set; }
        public int UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }
    }

    public class PerfilRequest
    {
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public int? NumeroPartidas { get; set; }
        public int? PartidasGanadas { get; set; }
        public int? PartidasPerdidas { get; set; }
        public string Avatar { get; set; }
        public DateTime? Nacido { get; set; }
        public int UsuarioId { get; set; }
    }
}
