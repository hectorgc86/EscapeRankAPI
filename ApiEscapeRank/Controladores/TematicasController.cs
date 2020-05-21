using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiEscapeRank.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ApiEscapeRank.Controladores
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TematicasController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public TematicasController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/Tematicas
        [HttpGet]
        public async Task<ActionResult<List<Tematica>>> GetTematicas()
        {
            List<Tematica> tematicas = await _contexto.GetTematicas().ToListAsync();

            if(tematicas == null)
            {
               return NotFound();
            }

            return tematicas;
        }

        // GET: api/Tematicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tematica>> GetTematica(string id)
        {
            Tematica tematica = await _contexto.GetTematica(id).FirstOrDefaultAsync();

            if (tematica == null)
            {
                return NotFound();
            }

            return tematica;
        }

        // PUT: api/Tematicas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTematica(string id, Tematica tematica)
        {
            if (id != tematica.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(tematica).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TematicaExists(id))
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

        // POST: api/Tematicas
        [HttpPost]
        public async Task<ActionResult<Tematica>> PostTematica(Tematica tematica)
        {
            _contexto.Tematicas.Add(tematica);
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TematicaExists(tematica.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTematicas", new { id = tematica.Id }, tematica);
        }

        // DELETE: api/Tematicas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Tematica>> DeleteTematica(string id)
        {
            var tematica = await _contexto.Tematicas.FindAsync(id);
            if (tematica == null)
            {
                return NotFound();
            }

            _contexto.Tematicas.Remove(tematica);
            await _contexto.SaveChangesAsync();

            return tematica;
        }

        private bool TematicaExists(string id)
        {
            return _contexto.Tematicas.Any(e => e.Id == id);
        }
    }
}
