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
    public class PerfilesController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public PerfilesController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/perfiles
        [HttpGet]
        public async Task<ActionResult<List<Perfil>>> GetPerfiles()
        {
            return await _contexto.Perfiles.ToListAsync();
        }

        // GET: api/perfiles/usuario/5/
        [HttpGet("usuario/{id}")]
        public async Task<ActionResult<Perfil>> GetPerfilUsuario(int id)
        {
            Perfil perfil = await _contexto.Perfiles
                .Where(ui=>ui.UsuarioId == id)
                .FirstAsync();

            if (perfil == null)
            {
                return NotFound("No se encuentra perfil para usuario con id " + id);
            }
            else
            {
                return perfil;
            }
        }

        // GET: api/perfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Perfil>> GetPerfil(int id)
        {
            var perfil = await _contexto.Perfiles.FindAsync(id);

            if (perfil == null)
            {
                return NotFound();
            }

            return perfil;
        }

        // PUT: api/perfiles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerfil(int id, Perfil perfil)
        {
            if (id != perfil.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(perfil).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerfilExists(id))
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

        // POST: api/perfiles
        [HttpPost]
        public async Task<ActionResult<Perfil>> PostPerfil(Perfil perfil)
        {
            _contexto.Perfiles.Add(perfil);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction("GetPerfiles", new { id = perfil.Id }, perfil);
        }

        // DELETE: api/perfiles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Perfil>> DeletePerfil(int id)
        {
            var perfil = await _contexto.Perfiles.FindAsync(id);
            if (perfil == null)
            {
                return NotFound();
            }

            _contexto.Perfiles.Remove(perfil);
            await _contexto.SaveChangesAsync();

            return perfil;
        }

        private bool PerfilExists(int id)
        {
            return _contexto.Perfiles.Any(e => e.Id == id);
        }
    }
}
