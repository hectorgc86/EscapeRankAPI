using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace ApiEscapeRank.Modelos
{
    public class Login
    {
        public string UsuarioId { get; private set; }
        public string TipoToken { get; private set; }
        public string ExpiraEn { get; private set; }
        public string TokenAcceso { get; private set; }

        public Login(int usuarioId, IConfiguration configuration)
        {
            UsuarioId = usuarioId.ToString();

            GenerarToken(configuration);
        }

        private void GenerarToken(IConfiguration configuration)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings").GetSection("Secret").Value);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, UsuarioId)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            TokenAcceso = tokenHandler.WriteToken(token);
            TipoToken = "Bearer";
            ExpiraEn = token.ValidTo.ToString();
        }
    }

    public class LoginRequest
    {
        public string Usuario { get; set; }
        public string Contrasenya { get; set; }
        public string Token { get; set; }
    }
}
