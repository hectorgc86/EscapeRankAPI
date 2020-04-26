using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiEscapeRank.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace ApiEscapeRank.Controladores
{
    [Authorize]
    [Route("api/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;
        private readonly AppSettings _appSettings;

        public LoginController(IOptions<AppSettings> appSettings, MySQLDbcontext contexto)
        {
            _contexto = contexto;
            _appSettings = appSettings.Value;
        }

        // POST: api/login
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult PostLogin([FromBody]Request req)
        {
            Usuario usuario = _contexto.Usuarios.SingleOrDefault(x => x.Email == req.Email && x.Contrasenya == req.Contrasenya);

            Login resp = new Login();

            if (usuario == null)
            {
                return BadRequest("Email o contraseña incorrecta");
            }
            else
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);

                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, usuario.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

                resp.TipoToken = "Bearer";
                resp.TokenAcceso = tokenHandler.WriteToken(token);
                resp.TokenRefresco = tokenHandler.WriteToken(token);
                resp.ExpiraEn = token.ValidTo.ToString();
                resp.IdUsuario = usuario.Id.ToString();

                return Ok(resp);
            }
        }

        [AllowAnonymous]
        [HttpPost("registro")]
        public async Task<ActionResult<Login>> PostRegistro([FromBody]Request req)
        {
            Usuario usuarioRegistro = new Usuario
            {
                Nombre = req.Nombre,
                Email = req.Email,
                Contrasenya = req.Contrasenya
            };

            _contexto.Usuarios.Add(usuarioRegistro);

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                //if (UsuarioExists(usuarioRegistro.Id))
                //{
                //    return Conflict();
                //}
                //else
                //{
                //    throw;
                //}
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            Login resp = new Login();

            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuarioRegistro.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            resp.TipoToken = "Bearer";
            resp.TokenAcceso = tokenHandler.WriteToken(token);
            resp.TokenRefresco = tokenHandler.WriteToken(token);
            resp.ExpiraEn = token.ValidTo.ToString();
            resp.IdUsuario = usuarioRegistro.Id.ToString();

            return resp;
        }
    }
}
