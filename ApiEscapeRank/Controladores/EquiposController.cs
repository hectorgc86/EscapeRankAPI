using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiEscapeRank.Interfaces;
using ApiEscapeRank.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiEscapeRank.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquiposController : ControllerBase, IEquiposService
    {
        private readonly MySQLDbcontext _contexto;

        public EquiposController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/equipos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipo>>> GetEquipos()
        {
            return await _contexto.Equipos.ToListAsync();
        }

        // GET: api/equipos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipo>> GetEquipo(int id)
        {
            var equipo = await _contexto.Equipos.FindAsync(id);

            if (equipo == null)
            {
                return NotFound();
            }

            return equipo;
        }

        // PUT: api/equipos/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipo(int id, Equipo equipo)
        {
            if (id != equipo.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(equipo).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipoExists(id))
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

        // POST: api/equipos
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Equipo>> PostEquipo(Equipo equipo)
        {
            _contexto.Equipos.Add(equipo);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction("GetEquipos", new { id = equipo.Id }, equipo);
        }

        // DELETE: api/equipos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Equipo>> DeleteEquipo(int id)
        {
            var equipo = await _contexto.Equipos.FindAsync(id);
            if (equipo == null)
            {
                return NotFound();
            }

            _contexto.Equipos.Remove(equipo);
            await _contexto.SaveChangesAsync();

            return equipo;
        }

        private bool EquipoExists(int id)
        {
            return _contexto.Equipos.Any(e => e.Id == id);
        }
    }
}
