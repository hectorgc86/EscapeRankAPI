using Newtonsoft.Json;

namespace ApiEscapeRank.Modelos
{
    public class Login
    {
        [JsonIgnore]
        public string Email { get; set; }
        [JsonIgnore]
        public string Contrasenya { get; set; }

        public string TipoToken { get; set; }
        public string ExpiraEn { get; set; }
        public string TokenAcceso { get; set; }
        public string TokenRefresco { get; set; }
        public string IdUsuario { get; set; }
    }
}
