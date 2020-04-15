using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiEscapeRank.Modelos;
using Microsoft.EntityFrameworkCore;

namespace ApiEscapeRank.Controladores
{
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
        public async Task<ActionResult<IEnumerable<Tematica>>> GetTematicas()
        {
            return await _contexto.Tematicas.ToListAsync();
        }

        // GET: api/Tematicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tematica>> GetTematica(string id)
        {
            var tematica = await _contexto.Tematicas.FindAsync(id);

            if (tematica == null)
            {
                return NotFound();
            }

            return tematica;
        }

        // PUT: api/Tematicas/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
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
