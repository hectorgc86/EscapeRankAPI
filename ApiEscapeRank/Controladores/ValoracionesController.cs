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
    public class ValoracionesController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public ValoracionesController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/Valoraciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Valoracion>>> GetValoraciones()
        {
            return await _contexto.Valoraciones.ToListAsync();
        }

        // GET: api/Valoraciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Valoracion>> GetValoracion(int id)
        {
            var valoracion = await _contexto.Valoraciones.FindAsync(id);

            if (valoracion == null)
            {
                return NotFound();
            }

            return valoracion;
        }

        // PUT: api/Valoraciones/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutValoracion(int id, Valoracion valoracion)
        {
            if (id != valoracion.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(valoracion).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValoracionExists(id))
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

        // POST: api/Valoraciones
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Valoracion>> PostValoracion(Valoracion valoracion)
        {
            _contexto.Valoraciones.Add(valoracion);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction("GetValoraciones", new { id = valoracion.Id }, valoracion);
        }

        // DELETE: api/Valoraciones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Valoracion>> DeleteValoracion(int id)
        {
            var valoracion = await _contexto.Valoraciones.FindAsync(id);
            if (valoracion == null)
            {
                return NotFound();
            }

            _contexto.Valoraciones.Remove(valoracion);
            await _contexto.SaveChangesAsync();

            return valoracion;
        }

        private bool ValoracionExists(int id)
        {
            return _contexto.Valoraciones.Any(e => e.Id == id);
        }
    }
}
