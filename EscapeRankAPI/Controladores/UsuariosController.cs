using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EscapeRankAPI.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace EscapeRankAPI.Controladores
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public UsuariosController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>Obtener todos los usuarios</summary>
        /// <response code="200">Usuarios devueltos</response>
        /// <response code="404">No se encuentran usuarios</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetUsuarios()
        {
            List<Usuario> usuarios = await _contexto.GetUsuarios().ToListAsync();

            if (usuarios == null)
            {
                return NotFound();
            }

            return usuarios;
        }

        /// <summary>Obtener todos los usuarios de un equipo</summary>
        /// <param name="id">Id de equipo</param>
        /// <response code="200">Usuarios de equipo devueltos</response>
        /// <response code="404">No se encuentran usuarios</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("equipo/{id}")]
        public async Task<ActionResult<List<Usuario>>> GetUsuariosEquipo(int id)
        {
            List<Usuario> usuariosEquipo = await _contexto.GetUsuariosEquipo(id).ToListAsync();

            if (usuariosEquipo == null)
            {
                return NotFound();
            }

            return usuariosEquipo;
        }

        /// <summary>Obtener un usuario por su id</summary>
        /// <param name="id">Id de usuario</param>
        /// <response code="200">Usuario devuelto</response>
        /// <response code="404">No se encuentra usuario</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            Usuario usuario = await _contexto.GetUsuario(id).FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound();
            }
            else
            {
                return usuario;
            } 
        }

        /// <summary>Obtener todos los amigos de un usuario</summary>
        /// <param name="id">Id de usuario</param>
        /// <response code="200">Amigos de usuario devueltos</response>
        /// <response code="404">No se encuentran amigos</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("{id}/amigos")]
        public ActionResult<List<Amigo>> GetAmigosUsuario(int id)
        {
            List<Amigo> amigos = _contexto.GetAmigosUsuario(id);

            if (amigos == null)
            {
                return NotFound();
            }
            else
            {
                return amigos;
            }
        }

        /// <summary>Añadir un nuevo usuario</summary>
        /// <param name="usuario">Usuario</param>
        /// <response code="200">Usuario añadido</response>
        /// <response code="409">Usuario ya existente</response>
        /// <response code="500">Error de servidor</response>
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _contexto.Usuarios.Add(usuario);

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return CreatedAtAction("GetUsuarios", new { id = usuario.Id }, usuario);
        }

        /// <summary>Mandar solicitud amistad</summary>
        /// <param name="usuarioId">Id de usuario que envia solicitud</param>
        /// <param name="emailAmigo">Email de usuario que recibe solicitud</param>
        /// <response code="200">Solicitud enviada</response>
        /// <response code="400">Parámetros incorrectos</response>
        /// <response code="404">Usuario destino no encontrado</response>
        /// <response code="500">Error de servidor</response>
        [HttpPost("{usuarioId}/amigos")]
        public async Task<ActionResult> PostAmigo(int usuarioId,[FromBody]string emailAmigo)
        {
            Usuario amigo = await _contexto.GetUsuarios()
                .Where(u => u.Email == emailAmigo).FirstOrDefaultAsync();

            if(amigo == null || amigo.Id == usuarioId)
            {
                return NotFound();
            }

            UsuariosAmigos usuarioAmigo = await _contexto.UsuariosAmigos
                .Where(u => u.UsuarioId == usuarioId && u.AmigoId == amigo.Id).FirstOrDefaultAsync();

            if (usuarioAmigo == null)
            {
                usuarioAmigo = new UsuariosAmigos()
                {
                    UsuarioId = usuarioId,
                    AmigoId = amigo.Id
                };

                _contexto.UsuariosAmigos.Add(usuarioAmigo);
            }
            else
            {
                usuarioAmigo.Estado = Estado.pendiente;

                _contexto.Entry(usuarioAmigo).State = EntityState.Modified;
            }
            
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return Ok();
        }

        /// <summary>Modificar un usuario por su id</summary>
        /// <param name="id">Id de usuario a modificar</param>
        /// <param name="usuario">Usuario modificado</param>
        /// <response code="200">Usuario modificado</response>
        /// <response code="400">Parámetros incorrectos</response>
        /// <response code="404">No se encuentra usuario</response>
        /// <response code="500">Error de servidor</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>Aceptar solicitud de amistad</summary>
        /// <param name="usuarioId">Id de usuario a aceptar</param>
        /// <param name="amigoId">Id de usuario que acepta</param>
        /// <response code="200">Solicitud aceptada</response>
        /// <response code="400">Parámetros incorrectos</response>
        /// <response code="404">No se encuentra solicitud amistad</response>
        /// <response code="500">Error de servidor</response>
        [HttpPut("{usuarioId}/amigos/{amigoId}")]
        public async Task<ActionResult> PutAmigo(int usuarioId, int amigoId)
        {
            UsuariosAmigos usuarioAmigo =
                await _contexto.UsuariosAmigos.Where(u => u.UsuarioId == usuarioId
                && u.AmigoId == amigoId || u.AmigoId == usuarioId
                && u.UsuarioId == amigoId).FirstOrDefaultAsync();

            if (usuarioAmigo == null)
            {
                return NotFound();
            }

            usuarioAmigo.Estado = Estado.aceptado;

            _contexto.Entry(usuarioAmigo).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return Ok();
        }

        /// <summary>Borrar un usuario</summary>
        /// <param name="id">Id de usuario</param>
        /// <response code="200">Usuario borrado</response>
        /// <response code="404">No se encuentra usuario</response>
        /// <response code="500">Error de servidor</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsuario(int id)
        {
            Usuario usuario = await _contexto.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _contexto.Usuarios.Remove(usuario);

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return Ok();
        }

        /// <summary>Borrar una amistad</summary>
        /// <param name="usuarioId">Id de usuario</param>
        /// <param name="amigoId">Id de amigo</param>
        /// <response code="200">Amistad borrada</response>
        /// <response code="404">No se encuentra amistad</response>
        /// <response code="500">Error de servidor</response>
        [HttpDelete("{usuarioId}/amigos/{amigoId}")]
        public async Task<ActionResult> DeleteAmigo(int usuarioId, int amigoId)
        {
            UsuariosAmigos usuarioAmigo =
                await _contexto.UsuariosAmigos.Where(u=>u.UsuarioId == usuarioId
                && u.AmigoId == amigoId || u.AmigoId == usuarioId
                && u.UsuarioId == amigoId).FirstOrDefaultAsync();

            if (usuarioAmigo == null)
            {
                return NotFound();
            }

            usuarioAmigo.Estado = Estado.borrado;

            _contexto.Entry(usuarioAmigo).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return Ok();
        }

        //Comprobar si un usuario existe
        private bool UsuarioExists(int id)
        {
            return _contexto.Usuarios.Any(e => e.Id == id);
        }

    }
}
