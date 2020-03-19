namespace ApiEscapeRank
{
    public partial class EquiposUsuarios
    {
        public int EquipoId { get; set; }
        public int UsuarioId { get; set; }

        public virtual Equipo Equipo { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
