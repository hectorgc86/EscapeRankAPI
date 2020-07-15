using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EscapeRankAPI.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace EscapeRankAPI.Controladores
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

        /// <summary>Iniciar sesión</summary>
        /// <param name="req">Datos para hacer login</param>
        /// <param name="nocript">En true se pude probar request con contraseña no encriptada</param>
        /// <response code="200">Devuelve id de usuario y token generado</response>
        /// <response code="400">Datos de login incorrectos</response>
        /// <response code="500">Error de servidor</response>
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

        /// <summary>Registrar nuevo usuario</summary>
        ///<param name="req">Usuario</param>
        /// <response code="200">Devuelve confirmación</response>
        /// <response code="409">Usuario ya existente</response>
        /// <response code="500">Error de servidor</response>
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
                if (RegistroExists(usuario))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
        }

        //Comprobar si existe un usuario
        private bool RegistroExists(Usuario usuario)
        {
            return _contexto.Usuarios.Any(e => e.Email == usuario.Email || e.Nick == usuario.Nick );
        }

        /* Encriptar contraseña (usado solo al poner nocript a true en login para poder probarlo desde postman y swagger
        sin tener que introducir una contraseña encriptada */
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
