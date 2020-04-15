using System.Collections.Generic;

namespace ApiEscapeRank.Modelos
{
    public partial class Ciudad
    {
        public Ciudad()
        {
            Companyias = new HashSet<Companyia>();
        }

        public string Id { get; set; }
        public string Nombre { get; set; }
        public string CiudadOrigen { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string ProvinciaId { get; set; }

        public virtual Provincia Provincia { get; set; }
        public virtual ICollection<Companyia> Companyias { get; set; }
    }
}
