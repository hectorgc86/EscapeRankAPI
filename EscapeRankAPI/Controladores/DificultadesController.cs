using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EscapeRankAPI.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace EscapeRankAPI.Controladores
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DificultadesController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public DificultadesController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>Obtener todas las dificultades</summary>
        /// <response code="200">Dificultades devueltas</response>
        /// <response code="404">No se encuentran dificultades</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet]
        public async Task<ActionResult<List<Dificultad>>> GetDificultades()
        {
            List<Dificultad> dificultades = await _contexto.GetDificultades().ToListAsync();

            if (dificultades == null)
            {
                return NotFound();
            }

            return dificultades;
        }

        /// <summary>Obtener una dificultad por su id</summary>
        /// <param name="id">Id de dificultad</param>
        /// <response code="200">Dificultad devuelta</response>
        /// <response code="404">No se encuentra dificultad</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Dificultad>> GetDificultad(string id)
        {
            Dificultad dificultad = await _contexto.GetDificultad(id).FirstOrDefaultAsync();

            if (dificultad == null)
            {
                return NotFound();
            }

            return dificultad;
        }

        /// <summary>Modificar una dificultad por su id</summary>
        /// <param name="id">Id de dificultad a modificar</param>
        /// <param name="dificultad">Dificultad modificada</param>
        /// <response code="200">Dificultad modificada</response>
        /// <response code="400">Parámetros incorrectos</response>
        /// <response code="404">No se encuentra dificultad</response>
        /// <response code="500">Error de servidor</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutDificultad(string id, Dificultad dificultad)
        {
            if (id != dificultad.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(dificultad).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DificultadExists(id))
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

        /// <summary>Añadir una nueva dificultad</summary>
        /// <param name="dificultad">Dificultad</param>
        /// <response code="200">Dificultad añadida</response>
        /// <response code="409">Dificultad ya existente</response>
        /// <response code="500">Error de servidor</response>
        [HttpPost]
        public async Task<ActionResult<Dificultad>> PostDificultades(Dificultad dificultad)
        {
            _contexto.Dificultades.Add(dificultad);
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DificultadExists(dificultad.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDificultades", new { id = dificultad.Id }, dificultad);
        }

        /// <summary>Borrar una dificultad</summary>
        /// <param name="id">Id de dificultad</param>
        /// <response code="200">Dificultad borrada</response>
        /// <response code="404">No se encuentra dificultad</response>
        /// <response code="500">Error de servidor</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Dificultad>> DeleteDificultad(string id)
        {
            var dificultad = await _contexto.Dificultades.FindAsync(id);
            if (dificultad == null)
            {
                return NotFound();
            }

            _contexto.Dificultades.Remove(dificultad);
            await _contexto.SaveChangesAsync();

            return dificultad;
        }

        //Comprobar si existe una dificultad
        private bool DificultadExists(string id)
        {
            return _contexto.Dificultades.Any(e => e.Id == id);
        }
    }
}
