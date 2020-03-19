using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiEscapeRank
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalasController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public SalasController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/salas
        [HttpGet]
        public async Task<ActionResult<List<Sala>>> GetSalas()
        {
            return await _contexto.Salas.Include(c=> c.Companyia).ToListAsync();
        }

   
        // GET: api/salas/conjunto/5
        [HttpGet("conjunto/{offset}")]
        public async Task<ActionResult<List<Sala>>> GetConjuntoSalas(int offset)
        {
            string sqlString = "SELECT * FROM salas LIMIT 10 OFFSET " + offset;

            List<Sala> salas = await _contexto.Salas.FromSqlRaw(sqlString).Include(c => c.Companyia).ToListAsync();

            if (salas == null)
            {
                return NotFound();
            }

            return Ok(salas);
        }

        // GET: api/salas/promocionadas/5
        [HttpGet("promocionadas/{offset}")]
        public async Task<ActionResult<List<Sala>>> GetSalasPromocionadas(int offset)
        {
            string sqlString = "SELECT * FROM salas WHERE promocionada = true LIMIT 10 OFFSET " + offset;

            List<Sala> salas = await _contexto.Salas.FromSqlRaw(sqlString).Include(c => c.Companyia).ToListAsync();

            if (salas == null)
            {
                return NotFound();
            }

            return Ok(salas);
        }

        // GET: api/salas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sala>> GetSala(string id)
        {
            var sala = await _contexto.Salas.Include(c => c.Companyia)
                .FirstOrDefaultAsync(i=>i.Id == id);

            if (sala == null)
            {
                return NotFound();
            }

            return sala;
        }

        // PUT: api/salas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSala(string id, Sala sala)
        {
            if (id != sala.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(sala).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalaExists(id))
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

       
        // POST: api/salas
        [HttpPost]
        public async Task<ActionResult<Sala>> PostSala(Sala sala)
        {
            _contexto.Salas.Add(sala);
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SalaExists(sala.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSalas", new { id = sala.Id }, sala);
        }

        // DELETE: api/salas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Sala>> DeleteSala(string id)
        {
            var sala = await _contexto.Salas.FindAsync(id);
            if (sala == null)
            {
                return NotFound();
            }

            _contexto.Salas.Remove(sala);
            await _contexto.SaveChangesAsync();

            return sala;
        }

        private bool SalaExists(string id)
        {
            return _contexto.Salas.Any(e => e.Id == id);
        }
    }
}
