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
    public class ValoracionesController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public ValoracionesController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>Obtener todas las valoraciones</summary>
        /// <response code="200">Valoraciones devueltas</response>
        /// <response code="404">No se encuentran valoraciones</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Valoracion>>> GetValoraciones()
        {
            return await _contexto.Valoraciones.ToListAsync();
        }

        /// <summary>Obtener una valoración por su id</summary>
        /// <param name="id">Id de valoración</param>
        /// <response code="200">Valoración devuelta</response>
        /// <response code="404">No se encuentra valoración</response>
        /// <response code="500">Error de servidor</response>
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

        /// <summary>Modificar una valoración por su id</summary>
        /// <param name="id">Id de valoración a modificar</param>
        /// <param name="valoracion">Valoración modificada</param>
        /// <response code="200">Valoración modificada</response>
        /// <response code="400">Parámetros incorrectos</response>
        /// <response code="404">No se encuentra valoración</response>
        /// <response code="500">Error de servidor</response>
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

        /// <summary>Añadir una nueva valoración</summary>
        /// <param name="valoracion">Valoración</param>
        /// <response code="200">Valoración añadida</response>
        /// <response code="409">Valoración ya existente</response>
        /// <response code="500">Error de servidor</response>
        [HttpPost]
        public async Task<ActionResult<Valoracion>> PostValoracion(Valoracion valoracion)
        {
            _contexto.Valoraciones.Add(valoracion);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction("GetValoraciones", new { id = valoracion.Id }, valoracion);
        }

        /// <summary>Borrar una valoración</summary>
        /// <param name="id">Id de valoración</param>
        /// <response code="200">Valoración borrada</response>
        /// <response code="404">No se encuentra valoración</response>
        /// <response code="500">Error de servidor</response>
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

        //Comprobar si existe una valoración
        private bool ValoracionExists(int id)
        {
            return _contexto.Valoraciones.Any(e => e.Id == id);
        }
    }
}
