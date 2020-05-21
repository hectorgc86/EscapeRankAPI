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
    public class PublicoController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public PublicoController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/Publico
        [HttpGet]
        public async Task<ActionResult<List<Publico>>> GetPublico()
        {
            List<Publico> publicos = await _contexto.GetPublico().ToListAsync();

            if (publicos == null)
            {
                return NotFound();
            }

            return publicos;
        }

        // GET: api/Publico/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Publico>> GetPublico(string id)
        {
            var publico = await _contexto.Publico.FindAsync(id);

            if (publico == null)
            {
                return NotFound();
            }

            return publico;
        }

        // PUT: api/Publico/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublico(string id, Publico publico)
        {
            if (id != publico.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(publico).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublicoExists(id))
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

        // POST: api/Publico
        [HttpPost]
        public async Task<ActionResult<Publico>> PostPublico(Publico publico)
        {
            _contexto.Publico.Add(publico);
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PublicoExists(publico.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPublico", new { id = publico.Id }, publico);
        }

        // DELETE: api/Publico/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Publico>> DeletePublico(string id)
        {
            var publico = await _contexto.Publico.FindAsync(id);
            if (publico == null)
            {
                return NotFound();
            }

            _contexto.Publico.Remove(publico);
            await _contexto.SaveChangesAsync();

            return publico;
        }

        private bool PublicoExists(string id)
        {
            return _contexto.Publico.Any(e => e.Id == id);
        }
    }
}
