using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiEscapeRank.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace ApiEscapeRank.Controladores
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TematicasController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public TematicasController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>Obtener todas las temáticas</summary>
        /// <response code="200">Temáticas devueltas</response>
        /// <response code="404">No se encuentran temáticas</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet]
        public async Task<ActionResult<List<Tematica>>> GetTematicas()
        {
            List<Tematica> tematicas = await _contexto.GetTematicas().ToListAsync();

            if(tematicas == null)
            {
               return NotFound();
            }

            return tematicas;
        }

        /// <summary>Obtener una temática por su id</summary>
        /// <param name="id">Id de temática</param>
        /// <response code="200">Temática devuelta</response>
        /// <response code="404">No se encuentra temática</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Tematica>> GetTematica(string id)
        {
            Tematica tematica = await _contexto.GetTematica(id).FirstOrDefaultAsync();

            if (tematica == null)
            {
                return NotFound();
            }

            return tematica;
        }

        /// <summary>Modificar una temática por su id</summary>
        /// <param name="id">Id de temática a modificar</param>
        /// <param name="tematica">Temática modificada    </param>
        /// <response code="200">Temática modificada</response>
        /// <response code="400">Parámetros incorrectos</response>
        /// <response code="404">No se encuentra temática</response>
        /// <response code="500">Error de servidor</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTematica(string id, Tematica tematica)
        {
            if (id != tematica.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(tematica).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TematicaExists(id))
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

        /// <summary>Añadir una nueva temática</summary>
        /// <param name="tematica">Temática</param>
        /// <response code="200">Temática añadida</response>
        /// <response code="409">Temática ya existente</response>
        /// <response code="500">Error de servidor</response>
        [HttpPost]
        public async Task<ActionResult<Tematica>> PostTematica(Tematica tematica)
        {
            _contexto.Tematicas.Add(tematica);
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TematicaExists(tematica.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTematicas", new { id = tematica.Id }, tematica);
        }

        /// <summary>Borrar una temática</summary>
        /// <param name="id">Id de temática</param>
        /// <response code="200">Temática borrada</response>
        /// <response code="404">No se encuentra temática</response>
        /// <response code="500">Error de servidor</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Tematica>> DeleteTematica(string id)
        {
            var tematica = await _contexto.Tematicas.FindAsync(id);
            if (tematica == null)
            {
                return NotFound();
            }

            _contexto.Tematicas.Remove(tematica);
            await _contexto.SaveChangesAsync();

            return tematica;
        }

        //Comprobar si existe temática
        private bool TematicaExists(string id)
        {
            return _contexto.Tematicas.Any(e => e.Id == id);
        }
    }
}
