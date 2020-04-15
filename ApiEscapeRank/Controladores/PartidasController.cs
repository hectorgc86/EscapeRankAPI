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
    public class PartidasController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public PartidasController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/partidas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Partida>>> GetPartidas()
        {
            return await _contexto.Partidas.ToListAsync();
        }

        // GET: api/partidas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Partida>> GetPartida(int id)
        {
            var partida = await _contexto.Partidas.FindAsync(id);

            if (partida == null)
            {
                return NotFound();
            }

            return partida;
        }

        // PUT: api/partidas/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPartida(int id, Partida partida)
        {
            if (id != partida.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(partida).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartidaExists(id))
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

        // POST: api/partidas
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Partida>> PostPartida(Partida partida)
        {
            _contexto.Partidas.Add(partida);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction("GetPartidas", new { id = partida.Id }, partida);
        }

        // DELETE: api/Partidas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Partida>> DeletePartida(int id)
        {
            var partida = await _contexto.Partidas.FindAsync(id);
            if (partida == null)
            {
                return NotFound();
            }

            _contexto.Partidas.Remove(partida);
            await _contexto.SaveChangesAsync();

            return partida;
        }

        private bool PartidaExists(int id)
        {
            return _contexto.Partidas.Any(e => e.Id == id);
        }
    }
}
