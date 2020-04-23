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
    public class ProvinciasController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public ProvinciasController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/provincias
        [HttpGet]
        public async Task<ActionResult<List<Provincia>>> GetProvincias()
        {
            return await _contexto.Provincias.ToListAsync();
        }

        // GET: api/provincias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Provincia>> GetProvincia(string id)
        {
            var provincia = await _contexto.Provincias.FindAsync(id);

            if (provincia == null)
            {
                return NotFound();
            }

            return provincia;
        }

        // PUT: api/Provincias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProvincia(string id, Provincia provincia)
        {
            if (id != provincia.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(provincia).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProvinciaExists(id))
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

        // POST: api/Provincias
        [HttpPost]
        public async Task<ActionResult<Provincia>> PostProvincia(Provincia provincia)
        {
            _contexto.Provincias.Add(provincia);
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProvinciaExists(provincia.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProvincias", new { id = provincia.Id }, provincia);
        }

        // DELETE: api/Provincias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Provincia>> DeleteProvincia(string id)
        {
            var provincia = await _contexto.Provincias.FindAsync(id);
            if (provincia == null)
            {
                return NotFound();
            }

            _contexto.Provincias.Remove(provincia);
            await _contexto.SaveChangesAsync();

            return provincia;
        }

        private bool ProvinciaExists(string id)
        {
            return _contexto.Provincias.Any(e => e.Id == id);
        }
    }
}
