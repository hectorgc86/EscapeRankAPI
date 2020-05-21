using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiEscapeRank.Modelos
{
    public partial class Usuario
    {
        [JsonIgnore]
        public string Contrasenya { get; set; }

        public int Id { get; set; }
        public string Nick { get; set; }
        public string Email { get; set; }
        public bool Activado { get; set; }
        public virtual Perfil Perfil { get; set; }

        public virtual ICollection<EquiposUsuarios> EquiposUsuarios { get; set; }
        public virtual ICollection<Noticia> Noticias { get; set; }
        public virtual ICollection<UsuariosAmigos> UsuariosAmigos{ get; set; }

        public Usuario()
        {
            EquiposUsuarios = new HashSet<EquiposUsuarios>();
            Noticias = new HashSet<Noticia>();
            UsuariosAmigos = new HashSet<UsuariosAmigos>();
        }
    }

    public class UsuarioRequest
    {
        public string Nick { get; set; }
        public string Contrasenya { get; set; }
        public string Email { get; set; }
        public PerfilRequest Perfil { get; set; }
    }

    public class Amigo : Usuario
    {
        public Estado Estado { get; set; }
    }
}
