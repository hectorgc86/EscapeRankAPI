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
    public class EquiposController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public EquiposController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/equipos
        [HttpGet]
        public async Task<ActionResult<List<Equipo>>> GetEquipos()
        {
            return await _contexto.Equipos.ToListAsync();
        }

        // GET: api/equipos/usuario/5
        [HttpGet("usuario/{id}")]
        public async Task<ActionResult<List<Equipo>>> GetEquiposUsuario(int id)
        {
            string consulta = "SELECT * FROM equipos WHERE id" +
                " IN(SELECT equipo_id FROM equipos_usuarios WHERE usuario_id = " + id + ")";

            List<Equipo> equipos = await _contexto.Equipos.FromSqlRaw(consulta).ToListAsync();

            if (equipos == null)
            {
                return NotFound("No se encuentran equipos para usuario con id " + id);
            }
            else
            {
                return equipos;
            }
        }

        // GET: api/equipos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipo>> GetEquipo(int id)
        {
            Equipo equipo = await _contexto.Equipos.FirstOrDefaultAsync(i => i.Id == id);

            if (equipo == null)
            {
                return NotFound();
            }

            return equipo;
        }

        // PUT: api/equipos/5
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
