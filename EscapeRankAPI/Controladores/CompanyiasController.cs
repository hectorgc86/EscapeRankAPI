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
    public class CompanyiasController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public CompanyiasController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>Obtener todas las compañías</summary>
        /// <response code="200">Compañías devueltas</response>
        /// <response code="404">No se encuentran compañías</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet]
        public async Task<ActionResult<List<Companyia>>> GetCompanyias()
        {
            List<Companyia> companyias = await _contexto.GetCompanyias().ToListAsync();

            if (companyias == null)
            {
                return NotFound();
            }

            return companyias;
        }

        /// <summary>Obtener una compañía por su id</summary>
        /// <param name="id">Id de compañía</param>
        /// <response code="200">Compañía devuelta</response>
        /// <response code="404">No se encuentra compañía</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Companyia>> GetCompanyia(string id)
        {
            Companyia companyia = await _contexto.GetCompanyia(id).FirstOrDefaultAsync();

            if (companyia == null)
            {
                return NotFound();
            }

            return companyia;
        }


        /// <summary>Modificar una compañía por su id</summary>
        /// <param name="id">Id de compañía a modificar</param>
        /// <param name="companyias">Compañía modificada</param>
        /// <response code="200">Compañía modificada</response>
        /// <response code="400">Parámetros incorrectos</response>
        /// <response code="404">No se encuentra compañía</response>
        /// <response code="500">Error de servidor</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCompanyia(string id, Companyia companyias)
        {
            if (id != companyias.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(companyias).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyiaExists(id))
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

        /// <summary>Añadir una nueva compañía</summary>
        /// <param name="companyia">Compañía</param>
        /// <response code="200">Compañía añadida</response>
        /// <response code="409">Compañía ya existente</response>
        /// <response code="500">Error de servidor</response>
        [HttpPost]
        public async Task<ActionResult<Companyia>> PostCompanyia(Companyia companyia)
        {
            _contexto.Companyias.Add(companyia);
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CompanyiaExists(companyia.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCompanyias", new { id = companyia.Id }, companyia);
        }

        /// <summary>Borrar una compañía</summary>
        /// <param name="id">Id de compañía</param>
        /// <response code="200">Compañía borrada</response>
        /// <response code="404">No se encuentra compañía</response>
        /// <response code="500">Error de servidor</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Companyia>> DeleteCompanyia(string id)
        {
            var companyia = await _contexto.Companyias.FindAsync(id);
            if (companyia == null)
            {
                return NotFound();
            }

            _contexto.Companyias.Remove(companyia);
            await _contexto.SaveChangesAsync();

            return companyia;
        }

        //Comprobar si existe una compañia
        private bool CompanyiaExists(string id)
        {
            return _contexto.Companyias.Any(e => e.Id == id);
        }
    }
}
