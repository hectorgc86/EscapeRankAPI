using System;

namespace ApiEscapeRank.Modelos
{
    public partial class Perfil
    {
        public int Id { get; set; }
        public string Nick { get; set; }
        public string Avatar { get; set; }
        public int? NumeroPartidas { get; set; }
        public int? PartidasGanadas { get; set; }
        public int? PartidasPerdidas { get; set; }
        public DateTime? MejorTiempo { get; set; }
        public int UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }

    }
}
