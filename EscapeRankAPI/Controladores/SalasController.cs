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
    public class SalasController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public SalasController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>Obtener todas las salas en base a paginado y búsqueda coincidente</summary>
        /// <param name="busqueda">Cadena de búsqueda</param>
        /// <param name="offset">Valor de paginado</param>
        /// <response code="200">Salas devueltas</response>
        /// <response code="404">No se encuentran salas</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet]
        public async Task<ActionResult<List<Sala>>> GetSalas([FromQuery]int offset,[FromQuery]string busqueda)
        {
            List<Sala> salas;

            if (string.IsNullOrEmpty(busqueda))
            {
                salas = await _contexto.GetSalas(offset).ToListAsync();
            }
            else
            {
                salas = await _contexto.GetSalasFiltradas(offset, busqueda).ToListAsync();
            }

            if (salas == null)
            {
                return NotFound();
            }

            return salas;
        }

        /// <summary>Obtener todas las salas promocionadas en base a paginado</summary>
        /// <param name="offset">Valor de paginado</param>
        /// <response code="200">Salas devueltas</response>
        /// <response code="404">No se encuentran salas</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("promocionadas")]
        public async Task<ActionResult<List<Sala>>> GetSalasPromocionadas([FromQuery]int offset)
        {
            List<Sala> salasPromocionadas = await _contexto.Salas.Where(p=>p.Promocionada == true)
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

        /// <summary>Obtener todas las salas de una categoría en base a su paginado y búsqueda coincidente</summary>
        /// <param name="id">Id de categoría</param>
        /// <param name="busqueda">Cadena de búsqueda</param>
        /// <param name="offset">Valor de paginado</param>
        /// <response code="200">Salas devueltas</response>
        /// <response code="404">No se encuentran salas</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("categoria/{id}")]
        public async Task<ActionResult<List<Sala>>> GetSalasCategoria(string id,[FromQuery]int offset,[FromQuery]string busqueda)
        {
            List<Sala> salasCategoria;

            if (string.IsNullOrEmpty(busqueda))
            {
                salasCategoria = await _contexto.GetSalasCategoria(id, offset).ToListAsync();
            }
            else
            {
                salasCategoria = await _contexto.GetSalasCategoriaFiltradas(id, offset, busqueda).ToListAsync();
            }

            if (salasCategoria == null)
            {
                return NotFound();
            }

            return salasCategoria;
        }

        /// <summary>Obtener todas las salas de una temática en base a su paginado y búsqueda coincidente</summary>
        /// <param name="id">Id de temática</param>
        /// <param name="busqueda">Cadena de búsqueda</param>
        /// <param name="offset">Valor de paginado</param>
        /// <response code="200">Salas devueltas</response>
        /// <response code="404">No se encuentran salas</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("tematica/{id}")]
        public async Task<ActionResult<List<Sala>>> GetSalasTematica(string id,[FromQuery]int offset,[FromQuery]string busqueda)
        {
            List<Sala> salasTematica;

            if (string.IsNullOrEmpty(busqueda))
            {
                salasTematica = await _contexto.GetSalasTematica(id, offset).ToListAsync();
            }
            else
            {
                salasTematica = await _contexto.GetSalasTematicaFiltradas(id, offset, busqueda).ToListAsync();
            }
                if (salasTematica == null)
            {
                return NotFound();
            }

            return salasTematica;
        }

        /// <summary>Obtener todas las salas de un público en base a su paginado y búsqueda coincidente</summary>
        /// <param name="id">Id de público</param>
        /// <param name="busqueda">Cadena de búsqueda</param>
        /// <param name="offset">Valor de paginado</param>
        /// <response code="200">Salas devueltas</response>
        /// <response code="404">No se encuentran salas</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("publico/{id}")]
        public async Task<ActionResult<List<Sala>>> GetSalasPublico(string id,[FromQuery]int offset,[FromQuery]string busqueda)
        {
            List<Sala> salasPublico;

            if (string.IsNullOrEmpty(busqueda))
            {
                salasPublico = await _contexto.GetSalasPublico(id, offset).ToListAsync();
            }
            else
            {
                salasPublico = await _contexto.GetSalasPublicoFiltradas(id, offset, busqueda).ToListAsync();
            }

            if (salasPublico == null)
            {
                return NotFound();
            }

            return salasPublico;
        }

        /// <summary>Obtener todas las salas de una dificultad en base a su paginado y búsqueda coincidente</summary>
        /// <param name="id">Id de dificultad</param>
        /// <param name="busqueda">Cadena de búsqueda</param>
        /// <param name="offset">Valor de paginado</param>
        /// <response code="200">Salas devueltas</response>
        /// <response code="404">No se encuentran salas</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("dificultad/{id}")]
        public async Task<ActionResult<List<Sala>>> GetSalasDificultad(string id,[FromQuery]int offset,[FromQuery]string busqueda)
        {
            List<Sala> salasDificultad;

            if (string.IsNullOrEmpty(busqueda))
            {
                salasDificultad = await _contexto.GetSalasDificultad(id, offset).ToListAsync();
            }
            else
            {
                salasDificultad = await _contexto.GetSalasDificultadFiltradas(id, offset, busqueda).ToListAsync();
            }

            if (salasDificultad == null)
            {
                return NotFound();
            }

            return salasDificultad;
        }


        /// <summary>Obtener una sala por su id</summary>
        /// <param name="id">Id de sala</param>
        /// <response code="200">Sala devuelta</response>
        /// <response code="404">No se encuentra sala</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Sala>> GetSala(string id)
        {
            Sala sala = await _contexto.GetSala(id).FirstOrDefaultAsync();

            if (sala == null)
            {
                return NotFound();
            }

            return sala;
        }

        /// <summary>Modificar una sala por su id</summary>
        /// <param name="id">Id de sala a modificar</param>
        /// <param name="sala">Sala modificada</param>
        /// <response code="200">Sala modificada</response>
        /// <response code="400">Parámetros incorrectos</response>
        /// <response code="404">No se encuentra sala</response>
        /// <response code="500">Error de servidor</response>
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


        /// <summary>Añadir una sala</summary>
        /// <param name="sala">Sala</param>
        /// <response code="200">Sala añadida</response>
        /// <response code="409">Sala ya existente</response>
        /// <response code="500">Error de servidor</response>
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

        /// <summary>Borrar una sala</summary>
        /// <param name="id">Id de sala</param>
        /// <response code="200">Sala borrada</response>
        /// <response code="404">No se encuentra sala</response>
        /// <response code="500">Error de servidor</response>
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

        //Comprobar si una sala existe
        private bool SalaExists(string id)
        {
            return _contexto.Salas.Any(e => e.Id == id);
        }
    }
}
