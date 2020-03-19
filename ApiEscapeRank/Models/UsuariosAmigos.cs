namespace ApiEscapeRank
{
    public partial class UsuariosAmigos
    {
        public int UsuarioId { get; set; }
        public int AmigoId { get; set; }

        public virtual Usuario Amigo { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
