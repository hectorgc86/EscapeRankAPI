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
    public class SalasController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public SalasController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/salas
        [HttpGet]
        public async Task<ActionResult<List<Sala>>> GetSalas([FromQuery]int offset,[FromQuery]string busqueda)
        {
            List<Sala> salas;

            if (string.IsNullOrEmpty(busqueda))
            {
                salas = await _contexto.Salas
                .Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                .Skip(offset)
                .Take(10)
                .ToListAsync();
            }
            else
            {
                salas = await _contexto.Salas
                .Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                .Where(s=>s.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                s.Companyia.Ciudad.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                s.Companyia.Nombre.ToLower().Contains(busqueda.ToLower()))
                .Skip(offset)
                .Take(10)
                .ToListAsync();
            }

            if (salas == null)
            {
                return NotFound();
            }

            return salas;
        }

        // GET: api/salas/promocionadas
        [HttpGet("promocionadas")]
        public async Task<ActionResult<List<Sala>>> GetSalasPromocionadas([FromQuery]int offset)
        {
            List<Sala> salasPromocionadas = await _contexto.Salas.Where(p=>p.Promocionada == 1)
                .Include(c => c.Companyia).ThenInclude(ci=>ci.Ciudad)
                .Skip(offset)
                .Take(10)
                .ToListAsync();

            if (salasPromocionadas == null)
            {
                return NotFound();
            }

            return salasPromocionadas;
        }

        // GET: api/salas/categoria/5
        [HttpGet("categoria/{id}")]
        public async Task<ActionResult<List<Sala>>> GetSalasCategoria(string id,[FromQuery]int offset,[FromQuery]string busqueda)
        {
            List<Sala> salasCategoria;

            if (string.IsNullOrEmpty(busqueda))
            {
                salasCategoria = await _contexto.Salas
                .Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                .Where(s => s.SalasCategorias.Any(c => c.CategoriaId == id))
                .Skip(offset)
                .Take(10)
                .ToListAsync();
            }
            else
            {
                salasCategoria = await _contexto.Salas
               .Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
               .Where(s => s.SalasCategorias.Any(c=> c.CategoriaId == id) &&
               s.Nombre.ToLower().Contains(busqueda.ToLower()) ||
               s.Companyia.Ciudad.Nombre.ToLower().Contains(busqueda.ToLower()) ||
               s.Companyia.Nombre.ToLower().Contains(busqueda.ToLower()))
               .Skip(offset)
               .Take(10)
               .ToListAsync();
            }

            if (salasCategoria == null)
            {
                return NotFound();
            }

            return salasCategoria;
        }

        // GET: api/salas/tematica/5
        [HttpGet("tematica/{id}")]
        public async Task<ActionResult<List<Sala>>> GetSalasTematica(string id,[FromQuery]int offset,[FromQuery]string busqueda)
        {
            List<Sala> salasTematica;

            if (string.IsNullOrEmpty(busqueda))
            {
                salasTematica = await _contexto.Salas
                .Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                .Where(p => p.SalasTematicas.Any(t => t.TematicaId == id))
                .Skip(offset)
                .Take(10)
                .ToListAsync();
            }
            else
            {
                salasTematica = await _contexto.Salas
                .Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                .Where(s => s.SalasTematicas.Any(t => t.TematicaId == id) &&
                s.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                s.Companyia.Ciudad.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                s.Companyia.Nombre.ToLower().Contains(busqueda.ToLower()))
                .Skip(offset)
                .Take(10)
                .ToListAsync();
                  
            }
                if (salasTematica == null)
            {
                return NotFound();
            }

            return salasTematica;
        }

        // GET: api/salas/publico/5
        [HttpGet("publico/{id}")]
        public async Task<ActionResult<List<Sala>>> GetSalasPublico(string id,[FromQuery]int offset,[FromQuery]string busqueda)
        {
            List<Sala> salasPublico;

            if (string.IsNullOrEmpty(busqueda))
            {
                salasPublico = await _contexto.Salas
                .Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                .Where(p => p.SalasPublico.Any(c => c.PublicoId == id))
                .Skip(offset)
                .Take(10)
                .ToListAsync();
            }
            else
            {
                salasPublico = await _contexto.Salas
               .Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
               .Where(s => s.SalasPublico.Any(c => c.PublicoId == id) &&
               s.Nombre.ToLower().Contains(busqueda.ToLower()) ||
               s.Companyia.Ciudad.Nombre.ToLower().Contains(busqueda.ToLower()) ||
               s.Companyia.Nombre.ToLower().Contains(busqueda.ToLower()))
               .Skip(offset)
               .Take(10)
               .ToListAsync();
            }

            if (salasPublico == null)
            {
                return NotFound();
            }

            return salasPublico;
        }

        // GET: api/salas/dificultad/5
        [HttpGet("dificultad/{id}")]
        public async Task<ActionResult<List<Sala>>> GetSalasDificultad(string id,[FromQuery]int offset,[FromQuery]string busqueda)
        {
            List<Sala> salasDificultad;

            if (string.IsNullOrEmpty(busqueda))
            {
                salasDificultad = await _contexto.Salas
                .Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                .Where(d => d.DificultadId == id)
                .Skip(offset)
                .Take(10)
                .ToListAsync();
            }
            else
            {
                salasDificultad = await _contexto.Salas
                .Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                .Where(s => s.DificultadId == id &&
                s.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                s.Companyia.Ciudad.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                s.Companyia.Nombre.ToLower().Contains(busqueda.ToLower()))
                .Skip(offset)
                .Take(10)
                .ToListAsync();
            }

            if (salasDificultad == null)
            {
                return NotFound();
            }

            return salasDificultad;
        }

        // GET: api/salas/provincia/5
        [HttpGet("provincia/{id}")]
        public async Task<ActionResult<List<Sala>>> GetSalasProvincia(string id,[FromQuery]int offset)
        {
            List<Sala> salasProvincia = await _contexto.Salas
                .Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                .Where(pi => pi.Companyia.Ciudad.ProvinciaId == id)
                .Skip(offset)
                .Take(10)
                .ToListAsync();

            if (salasProvincia == null)
            {
                return NotFound();
            }

            return salasProvincia;
        }

        // GET: api/salas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sala>> GetSala(string id)
        {
            var sala = await _contexto.Salas
                .Include(e=>e.Dificultad)
                .Include(c => c.Companyia).ThenInclude(ci => ci.Ciudad)
                .Include(sc => sc.SalasCategorias).ThenInclude(s=>s.Categoria)
                .Include(st => st.SalasTematicas).ThenInclude(t=>t.Tematica)
                .Include(sp => sp.SalasPublico).ThenInclude(p=>p.Publico)
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
