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
    public class ProvinciasController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public ProvinciasController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>Obtener todas las provincias</summary>
        /// <response code="200">Provincias devueltas</response>
        /// <response code="404">No se encuentran provincias</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet]
        public async Task<ActionResult<List<Provincia>>> GetProvincias()
        {
            return await _contexto.Provincias.ToListAsync();
        }

        /// <summary>Obtener una provincia por su id</summary>
        /// <param name="id">Id de provincia</param>
        /// <response code="200">Provincia devuelta</response>
        /// <response code="404">No se encuentra provincia</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Provincia>> GetProvincia(string id)
        {
            var provincia = await _contexto.Provincias.FindAsync(id);

            if (provincia == null)
            {
                return NotFound();
            }

            return provincia;
        }

        /// <summary>Modificar una provincia por su id</summary>
        /// <param name="id">Id de provincia a modificar</param>
        /// <param name="provincia">Provincia modificada</param>
        /// <response code="200">Provincia modificada</response>
        /// <response code="400">Parámetros incorrectos</response>
        /// <response code="404">No se encuentra provincia</response>
        /// <response code="500">Error de servidor</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProvincia(string id, Provincia provincia)
        {
            if (id != provincia.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(provincia).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProvinciaExists(id))
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

        /// <summary>Añadir una nueva provincia</summary>
        /// <param name="provincia">Provincia</param>
        /// <response code="200">Provincia añadida</response>
        /// <response code="409">Provincia ya existente</response>
        /// <response code="500">Error de servidor</response>
        [HttpPost]
        public async Task<ActionResult<Provincia>> PostProvincia(Provincia provincia)
        {
            _contexto.Provincias.Add(provincia);
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProvinciaExists(provincia.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProvincias", new { id = provincia.Id }, provincia);
        }

        /// <summary>Borrar una provincia</summary>
        /// <param name="id">Id de provincia</param>
        /// <response code="200">Provincia borrada</response>
        /// <response code="404">No se encuentra provincia</response>
        /// <response code="500">Error de servidor</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Provincia>> DeleteProvincia(string id)
        {
            var provincia = await _contexto.Provincias.FindAsync(id);
            if (provincia == null)
            {
                return NotFound();
            }

            _contexto.Provincias.Remove(provincia);
            await _contexto.SaveChangesAsync();

            return provincia;
        }

        //Comprobar si una provincia existe
        private bool ProvinciaExists(string id)
        {
            return _contexto.Provincias.Any(e => e.Id == id);
        }
    }
}
