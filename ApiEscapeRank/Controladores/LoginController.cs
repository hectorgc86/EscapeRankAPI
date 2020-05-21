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
        private readonly IConfiguration _configuration;

        public LoginController(MySQLDbcontext contexto, IConfiguration configuration)
        {
            _contexto = contexto;
            _configuration = configuration;
        }

        // POST: api/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> PostLogin([FromBody]LoginRequest req,[FromHeader] bool nocript)
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

            Usuario usuario = await _contexto.Usuarios.SingleOrDefaultAsync
                (x => x.Email == req.Usuario && x.Contrasenya == contrasenya ||
                x.Nick == req.Usuario && x.Contrasenya == contrasenya);

            if (usuario == null)
            {
                return BadRequest();
            }
            else
            {
                Login resp = new Login(usuario.Id, _configuration);

                return Ok(resp);
            }
        }

        // POST: api/registro
        [AllowAnonymous]
        [HttpPost("registro")]
        public async Task<ActionResult<Login>> PostRegistro([FromBody]UsuarioRequest req)
        {
            Usuario usuario = new Usuario
            {
                Nick = req.Nick,
                Email = req.Email,
                Contrasenya = req.Contrasenya
            };

            usuario.Perfil = new Perfil
            {
                Nombre = req.Perfil.Nombre,
                Telefono = req.Perfil.Telefono,
                Avatar = "https://picsum.photos/200/300?random=",
                NumeroPartidas = 0,
                PartidasGanadas = 0,
                PartidasPerdidas = 0,
                Nacido = req.Perfil.Nacido,
                UsuarioId = usuario.Id
            };

            _contexto.Usuarios.Add(usuario);

            try
            {
                await _contexto.SaveChangesAsync();

                Login resp = new Login(usuario.Id, _configuration);

                return resp;
            }
            catch (DbUpdateException)
            {
                if (RegistroExists(usuario.Email))
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
