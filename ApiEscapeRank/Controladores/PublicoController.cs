using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiEscapeRank.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

/* Héctor Granja Cortés
 * 2ºDAM Semipresencial
 * Proyecto fin de ciclo
   EscapeRank API */

namespace ApiEscapeRank.Controladores
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PublicoController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public PublicoController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>Obtener todos los públicos</summary>
        /// <response code="200">Públicos devueltos</response>
        /// <response code="404">No se encuentran públicos</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet]
        public async Task<ActionResult<List<Publico>>> GetPublico()
        {
            List<Publico> publicos = await _contexto.GetPublico().ToListAsync();

            if (publicos == null)
            {
                return NotFound();
            }

            return publicos;
        }

        /// <summary>Obtener un público por su id</summary>
        /// <param name="id">Id de público</param>
        /// <response code="200">Publico devuelto</response>
        /// <response code="404">No se encuentra público</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Publico>> GetPublico(string id)
        {
            var publico = await _contexto.Publico.FindAsync(id);

            if (publico == null)
            {
                return NotFound();
            }

            return publico;
        }

        /// <summary>Modificar un público por su id</summary>
        /// <param name="id">Id de público a modificar</param>
        /// <param name="público">Público modificado</param>
        /// <response code="200">Público modificado</response>
        /// <response code="400">Parámetros incorrectos</response>
        /// <response code="404">No se encuentra público</response>
        /// <response code="500">Error de servidor</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublico(string id, Publico publico)
        {
            if (id != publico.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(publico).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublicoExists(id))
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

        /// <summary>Añadir un nuevo público</summary>
        /// <param name="publico">Público</param>
        /// <response code="200">Público añadido</response>
        /// <response code="409">Público ya existente</response>
        /// <response code="500">Error de servidor</response>
        [HttpPost]
        public async Task<ActionResult<Publico>> PostPublico(Publico publico)
        {
            _contexto.Publico.Add(publico);
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PublicoExists(publico.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPublico", new { id = publico.Id }, publico);
        }

        /// <summary>Borrar un público</summary>
        /// <param name="id">Id de público</param>
        /// <response code="200">Público borrada</response>
        /// <response code="404">No se encuentra público</response>
        /// <response code="500">Error de servidor</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Publico>> DeletePublico(string id)
        {
            var publico = await _contexto.Publico.FindAsync(id);
            if (publico == null)
            {
                return NotFound();
            }

            _contexto.Publico.Remove(publico);
            await _contexto.SaveChangesAsync();

            return publico;
        }

        //Comprobar si un público existe
        private bool PublicoExists(string id)
        {
            return _contexto.Publico.Any(e => e.Id == id);
        }
    }
}
