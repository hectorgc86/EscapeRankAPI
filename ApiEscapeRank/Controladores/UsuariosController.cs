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
            return await _contexto.Usuarios.Include(p => p.Perfil).ToListAsync();
        }

        // GET: api/usuarios/equipo/5
        [HttpGet("equipo/{id}")]
        public async Task<ActionResult<List<Usuario>>> GetUsuariosEquipo(int id)
        {
            string sqlString = "SELECT * FROM usuarios WHERE id IN (SELECT usuario_id FROM equipos_usuarios WHERE equipo_id = " + id +")";

            List<Usuario> usuariosEquipo = await _contexto.Usuarios.FromSqlRaw(sqlString).ToListAsync();

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
            Usuario usuario = await _contexto.Usuarios
                .Include(p => p.Perfil)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (usuario == null)
            {
                return NotFound("No se encuentra usuario con id " + id);
            }
            else
            {
                return usuario;
            } 
        }


        // GET: api/usuarios/5/amigos
        [HttpGet("{id}/amigos")]
        public async Task<ActionResult<List<Usuario>>> GetAmigosUsuario(int id)
        {
            string consulta = "SELECT * FROM usuarios WHERE id" +
                " IN(SELECT amigo_id FROM usuarios_amigos WHERE usuario_id = " + id + ")";

            List<Usuario> amigos = await _contexto.Usuarios.FromSqlRaw(consulta)
                .Include(p => p.Perfil)
                .ToListAsync();

            if (amigos == null)
            {
                return NotFound("No se encuentran amigos para usuario con id " + id);
            }
            else
            {
                return amigos;
            }
        }


        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
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

        // POST: api/usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _contexto.Usuarios.Add(usuario);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction("GetUsuarios", new { id = usuario.Id }, usuario);
        }

        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuario>> DeleteUsuario(int id)
        {
            var usuario = await _contexto.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _contexto.Usuarios.Remove(usuario);
            await _contexto.SaveChangesAsync();

            return usuario;
        }

        private bool UsuarioExists(int id)
        {
            return _contexto.Usuarios.Any(e => e.Id == id);
        }

    }
}
