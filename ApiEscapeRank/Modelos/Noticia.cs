using System;

namespace ApiEscapeRank.Modelos
{
    public partial class Noticia
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Titular { get; set; }
        public string TextoCorto { get; set; }
        public string TextoLargo { get; set; }
        public string Imagen { get; set; }
        public string Link { get; set; }
        public bool? Promocionada { get; set; }
        public int? NumeroFavoritos { get; set; }
        public int? NumeroComentarios { get; set; }
        public string CompanyiaId { get; set; }
        public int? UsuarioId { get; set; }

        public virtual Companyia Companyia { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
