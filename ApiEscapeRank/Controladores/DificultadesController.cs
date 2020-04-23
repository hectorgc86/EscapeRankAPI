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
    public class DificultadesController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public DificultadesController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/dificultades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dificultad>>> GetDificultades()
        {
            return await _contexto.Dificultades.ToListAsync();
        }

        // GET: api/dificultades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dificultad>> GetDificultad(string id)
        {
            var dificultad = await _contexto.Dificultades.FindAsync(id);

            if (dificultad == null)
            {
                return NotFound();
            }

            return dificultad;
        }

        // PUT: api/dificultades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDificultad(string id, Dificultad dificultad)
        {
            if (id != dificultad.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(dificultad).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DificultadExists(id))
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

        // POST: api/dificultades
        [HttpPost]
        public async Task<ActionResult<Dificultad>> PostDificultades(Dificultad dificultad)
        {
            _contexto.Dificultades.Add(dificultad);
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DificultadExists(dificultad.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDificultades", new { id = dificultad.Id }, dificultad);
        }

        // DELETE: api/dificultades/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Dificultad>> DeleteDificultad(string id)
        {
            var dificultad = await _contexto.Dificultades.FindAsync(id);
            if (dificultad == null)
            {
                return NotFound();
            }

            _contexto.Dificultades.Remove(dificultad);
            await _contexto.SaveChangesAsync();

            return dificultad;
        }

        private bool DificultadExists(string id)
        {
            return _contexto.Dificultades.Any(e => e.Id == id);
        }
    }
}
