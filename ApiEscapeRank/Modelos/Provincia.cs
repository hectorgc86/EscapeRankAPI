using System.Collections.Generic;

namespace ApiEscapeRank.Modelos
{
    public partial class Provincia
    {
        public Provincia()
        {
            Ciudades = new HashSet<Ciudad>();
        }

        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }

        public virtual ICollection<Ciudad> Ciudades { get; set; }
    }
}
