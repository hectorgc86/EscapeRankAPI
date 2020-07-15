using System.Collections.Generic;
using Newtonsoft.Json;

/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace EscapeRankAPI.Modelos
{
    public partial class Sala
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public bool Promocionada { get; set; }
        public string Duracion { get; set; }
        public int? MinimoJugadores { get; set; }
        public int? MaximoJugadores { get; set; }
        public string PrecioMinimo { get; set; }
        public string Descripcion { get; set; }
        public string PrecioMaximo { get; set; }
        public string UrlReserva { get; set; }
        public string EdadPublico { get; set; }
        public string Proximamente { get; set; }
        public string Visto { get; set; }
        public string ModoCombate { get; set; }
        public string TextoCombate { get; set; }
        public string SalaIgual { get; set; }
        public string EnOferta { get; set; }
        public string TextoOferta { get; set; }
        public string NumeroResenyas { get; set; }
        public string RegaloBonus { get; set; }
        public string DisponibleIngles { get; set; }
        public string AdaptadoMinusvalidos { get; set; }
        public string AdaptadoCiegos { get; set; }
        public string AdaptadoSordos { get; set; }
        public string AdaptadoEmbarazadas { get; set; }
        public string NoClaustrofobicos { get; set; }
        public string ImagenAncha { get; set; }
        public string ImagenEstrecha { get; set; }
        public string JugadoresIncluidos { get; set; }
        public string PrecioJugadorAdicional { get; set; }
        public string Validez { get; set; }
        public string ComoReservar { get; set; }
        public string TerminosReserva { get; set; }
        public string OtrosDatos { get; set; }
        public string CompanyiaId { get; set; }
        public string DificultadId { get; set; }

        public virtual Companyia Companyia { get; set; }
        public virtual Dificultad Dificultad { get; set; }
        public virtual ICollection<Partida> Partidas { get; set; }
        public virtual ICollection<SalasCategorias> SalasCategorias { get; set; }
        public virtual ICollection<SalasPublico> SalasPublico { get; set; }
        public virtual ICollection<SalasTematicas> SalasTematicas { get; set; }
        public virtual ICollection<Valoracion> Valoraciones { get; set; }

        public Sala()
        {
            Partidas = new HashSet<Partida>();
            SalasCategorias = new HashSet<SalasCategorias>();
            SalasPublico = new HashSet<SalasPublico>();
            SalasTematicas = new HashSet<SalasTematicas>();
            Valoraciones = new HashSet<Valoracion>();
        }
    }
}
