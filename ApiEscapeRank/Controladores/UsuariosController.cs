using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiEscapeRank.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiEscapeRank.Controladores
{
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

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            Usuario usuario = await _contexto.Usuarios.Include(p => p.Perfil)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (usuario == null)
            {
                return NotFound("No se encuentra usuario con id " + id);
            }
            else
            {
                return Ok(usuario);
            } 
        }

        // GET: api/usuarios/5/noticias
        [HttpGet("{id}/noticias")]
        public async Task<ActionResult<List<Noticia>>> GetNoticiasUsuario(int id)
        {
            string consulta = "SELECT * FROM noticias WHERE usuario_id" +
                " IN(SELECT amigo_id FROM usuarios_amigos WHERE usuario_id = "+ id + ")"+
                " OR noticias.promocionada = 1" +
                " OR noticias.usuario_id = "+ id + " ORDER by noticias.fecha DESC";

            List<Noticia> noticias = await _contexto.Noticias.FromSqlRaw(consulta).ToListAsync();

            if (noticias == null)
            {
                return NotFound("No se encuentran noticias para usuario con id " + id);
            }
            else
            {
                return Ok(noticias);
            }   
        }

        // GET: api/usuarios/5/amigos
        [HttpGet("{id}/amigos")]
        public async Task<ActionResult<List<Usuario>>> GetAmigosUsuario(int id)
        {
            string consulta = "SELECT * FROM usuarios WHERE id" +
                " IN(SELECT amigo_id FROM usuarios_amigos WHERE usuario_id = " + id + ")";

            List<Usuario> amigos = await _contexto.Usuarios.FromSqlRaw(consulta).Include(p => p.Perfil).ToListAsync();

            if (amigos == null)
            {
                return NotFound("No se encuentran amigos para usuario con id " + id);
            }
            else
            {
                return Ok(amigos);
            }
        }

        // GET: api/usuarios/5/equipos
        [HttpGet("{id}/equipos")]
        public async Task<ActionResult<List<Equipo>>> GetEquiposUsuario(int id)
        {
            string consulta = "SELECT * FROM equipos WHERE id" +
                " IN(SELECT equipo_id FROM equipos_usuarios WHERE usuario_id = " + id + ")";

            List<Equipo> equipos = await _contexto.Equipos.FromSqlRaw(consulta).ToListAsync();

            if (equipos == null)
            {
                return NotFound("No se encuentran equipos para usuario con id " + id);
            }
            else
            {
                return Ok(equipos);
            }
        }

        // GET: api/usuarios/5/perfil
        [HttpGet("{id}/perfil")]
        public async Task<ActionResult<Perfil>> GetPerfilUsuario(int id)
        {
            string consulta = "SELECT * FROM perfiles WHERE usuario_id = " + id;

            Perfil perfil = await _contexto.Perfiles.FromSqlRaw(consulta).FirstAsync();

            if (perfil == null)
            {
                return NotFound("No se encuentra perfil para usuario con id " + id);
            }
            else
            {
                return Ok(perfil);
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

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _contexto.Usuarios.Add(usuario);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction("GetUsuarios", new { id = usuario.Id }, usuario);
        }

        // DELETE: api/Usuarios/5
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
