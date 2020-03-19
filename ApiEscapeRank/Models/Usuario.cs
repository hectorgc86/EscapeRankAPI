using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiEscapeRank
{
    public partial class Usuario
    {
        public Usuario()
        {
            EquiposUsuarios = new HashSet<EquiposUsuarios>();
            Noticias = new HashSet<Noticia>();
            UsuariosAmigosAmigo = new HashSet<UsuariosAmigos>();
            UsuariosAmigosUsuario = new HashSet<UsuariosAmigos>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string Contrasenya { get; set; }

        public DateTime? Nacido { get; set; }
        public string Telefono { get; set; }
        public int Activado { get; set; }
        public int? CodigoActivado { get; set; }
        public string Avatar { get; set; }
        public virtual Perfil Perfil { get; set; }

        public virtual ICollection<EquiposUsuarios> EquiposUsuarios { get; set; }
        public virtual ICollection<Noticia> Noticias { get; set; }
        public virtual ICollection<UsuariosAmigos> UsuariosAmigosAmigo { get; set; }
        public virtual ICollection<UsuariosAmigos> UsuariosAmigosUsuario { get; set; }
    }
}
