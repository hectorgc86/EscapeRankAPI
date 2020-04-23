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
    public class CiudadesController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public CiudadesController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/ciudades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ciudad>>> GetCiudades()
        {
            return await _contexto.Ciudades.ToListAsync();
        }

        // GET: api/ciudades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ciudad>> GetCiudad(string id)
        {
            var ciudad = await _contexto.Ciudades.FindAsync(id);

            if (ciudad == null)
            {
                return NotFound();
            }

            return Ok(ciudad);
        }

        // PUT: api/ciudades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCiudad(string id, Ciudad ciudad)
        {
            if (id != ciudad.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(ciudad).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CiudadExists(id))
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

        // POST: api/ciudades
        [HttpPost]
        public async Task<ActionResult<Ciudad>> PostCiudad(Ciudad ciudad)
        {
            _contexto.Ciudades.Add(ciudad);
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CiudadExists(ciudad.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCiudades", new { id = ciudad.Id }, ciudad);
        }

        // DELETE: api/ciudades/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ciudad>> DeleteCiudad(string id)
        {
            var ciudad = await _contexto.Ciudades.FindAsync(id);
            if (ciudad == null)
            {
                return NotFound();
            }

            _contexto.Ciudades.Remove(ciudad);
            await _contexto.SaveChangesAsync();

            return ciudad;
        }

        private bool CiudadExists(string id)
        {
            return _contexto.Ciudades.Any(e => e.Id == id);
        }
    }
}
