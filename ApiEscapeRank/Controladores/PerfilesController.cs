using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiEscapeRank.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiEscapeRank.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilesController : ControllerBase, IPerfilesService
    {
        private readonly MySQLDbcontext _contexto;

        public PerfilesController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/perfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Perfil>>> GetPerfiles()
        {
            return await _contexto.Perfiles.ToListAsync();
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
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
