using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ApiEscapeRank.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ApiEscapeRank.Controladores
{
    [Authorize]
    [Route("api/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;
        private readonly IConfiguration Configuration;

        public LoginController(MySQLDbcontext contexto, IConfiguration configuration)
        {
            Configuration = configuration;
            _contexto = contexto;
        }

        // POST: api/login
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult PostLogin([FromBody]LoginRequest req,[FromHeader] bool nocript)
        {
            string contrasenya = "";

            if (nocript)
            {
                contrasenya = CalcularMD5(req.Contrasenya);
            }
            else
            {
                contrasenya = req.Contrasenya;
            }

           

            Usuario usuario = _contexto.Usuarios.SingleOrDefault(x => x.Email == req.Email && x.Contrasenya == contrasenya);

            if (usuario == null)
            {
                return BadRequest("Email o contraseña incorrecta");
            }
            else
            {
                Login resp = new Login(usuario.Id, Configuration);

                return Ok(resp);
            }
        }

        [AllowAnonymous]
        [HttpPost("registro")]
        public async Task<ActionResult<Login>> PostRegistro([FromBody]LoginRequest req)
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

                Login resp = new Login(usuarioRegistro.Id, Configuration);

                return resp;
            }
            catch (DbUpdateException)
            {
                if (RegistroExists(usuarioRegistro.Email))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool RegistroExists(string email)
        {
            return _contexto.Usuarios.Any(e => e.Email == email);
        }

        public static string CalcularMD5(string contrasenya)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(contrasenya);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
