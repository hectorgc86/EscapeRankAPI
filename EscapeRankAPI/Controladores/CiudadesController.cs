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
    public class CiudadesController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public CiudadesController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>Obtener todas las ciudades</summary>
        /// <response code="200">Ciudades devueltas</response>
        /// <response code="404">No se encuentran ciudades</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet]
        public async Task<ActionResult<List<Ciudad>>> GetCiudades()
        {
            List<Ciudad> ciudades = await _contexto.GetCiudades().ToListAsync();

            if (ciudades == null)
            {
                return NotFound();
            }

            return ciudades;
        }

        /// <summary>Obtener una ciudad por su id</summary>
        /// <param name="id">Id de ciudad</param>
        /// <response code="200">Ciudad devuelta</response>
        /// <response code="404">No se encuentra ciudad</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Ciudad>> GetCiudad(string id)
        {
            Ciudad ciudad = await _contexto.GetCiudad(id).FirstOrDefaultAsync();

            if (ciudad == null)
            {
                return NotFound();
            }

            return Ok(ciudad);
        }

        /// <summary>Modificar una ciudad por su id</summary>
        /// <param name="id">Id de ciudad a modificar</param>
        /// <param name="ciudad">Ciudad modificada</param>
        /// <response code="200">Ciudad modificada</response>
        /// <response code="400">Parámetros incorrectos</response>
        /// <response code="404">No se encuentra ciudad</response>
        /// <response code="500">Error de servidor</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCiudad(string id, Ciudad ciudad)
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

        /// <summary>Añadir una nueva ciudad</summary>
        /// <param name="ciudad">Ciudad</param>
        /// <response code="200">Ciudad añadida</response>
        /// <response code="409">Ciudad ya existente</response>
        /// <response code="500">Error de servidor</response>
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

        /// <summary>Borrar una ciudad</summary>
        /// <param name="id">Id de ciudad</param>
        /// <response code="200">Ciudad borrada</response>
        /// <response code="404">No se encuentra ciudad</response>
        /// <response code="500">Error de servidor</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ciudad>> DeleteCiudad(string id)
        {
            Ciudad ciudad = await _contexto.Ciudades.FindAsync(id);

            if (ciudad == null)
            {
                return NotFound();
            }

            _contexto.Ciudades.Remove(ciudad);
            await _contexto.SaveChangesAsync();

            return ciudad;
        }

        //Comprobar si existe una ciudad
        private bool CiudadExists(string id)
        {
            return _contexto.Ciudades.Any(e => e.Id == id);
        }
    }
}
