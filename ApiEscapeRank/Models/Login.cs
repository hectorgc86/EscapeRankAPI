using Newtonsoft.Json;

namespace ApiEscapeRank
{
    public class Login
    {
        [JsonIgnore]
        public string Usuario { get; set; }
        [JsonIgnore]
        public string Contrasenya { get; set; }

        public string TipoToken { get; set; }
        public string ExpiraEn { get; set; }
        public string TokenAcceso { get; set; }
        public string TokenRefresco { get; set; }
        public string IdUsuario { get; set; }
    }
}
