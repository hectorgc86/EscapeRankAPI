using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiEscapeRank.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiEscapeRank.Controladores
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

        // GET: api/usuarios
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

        // GET: api/usuarios/equipo/5
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

        // GET: api/usuarios/5
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

        // GET: api/usuarios/5/amigos
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

        // POST: api/usuarios
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

        // POST: api/usuarios/5/amigos
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

        // PUT: api/usuarios/5
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

        // PUT: api/usuarios/5/amigos/5
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

        // DELETE: api/usuarios/5
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

        // DELETE: api/usuarios/5/amigos/5
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

        private bool UsuarioExists(int id)
        {
            return _contexto.Usuarios.Any(e => e.Id == id);
        }

    }
}
