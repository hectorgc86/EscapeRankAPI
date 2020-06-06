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
    public class PerfilesController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public PerfilesController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>Obtener todos los perfiles</summary>
        /// <response code="200">Perfiles devueltos</response>
        /// <response code="404">No se encuentran perfiles</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet]
        public async Task<ActionResult<List<Perfil>>> GetPerfiles()
        {
            return await _contexto.Perfiles.ToListAsync();
        }

        /// <summary>Obtener perfil de un usuario</summary>
        /// <param name="id">Id de usuario</param>
        /// <response code="200">Perfil de usuario devuelto</response>
        /// <response code="404">No se encuentra perfil</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("usuario/{id}")]
        public async Task<ActionResult<Perfil>> GetPerfilUsuario(int id)
        {
            Perfil perfil = await _contexto.Perfiles
                .Where(ui=>ui.UsuarioId == id)
                .FirstAsync();

            if (perfil == null)
            {
                return NotFound();
            }
            else
            {
                return perfil;
            }
        }

        /// <summary>Obtener un perfil por su id</summary>
        /// <param name="id">Id de perfil</param>
        /// <response code="200">Perfil devuelto</response>
        /// <response code="404">No se encuentra perfil</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Perfil>> GetPerfil(int id)
        {
            var perfil = await _contexto.Perfiles.FindAsync(id);

            if (perfil == null)
            {
                return NotFound();
            }

            return perfil;
        }

        /// <summary>Modificar un perfil por su id</summary>
        /// <param name="id">Id de perfil a modificar</param>
        /// <param name="perfil">Perfil modificado</param>
        /// <response code="200">Perfil modificado</response>
        /// <response code="400">Parámetros incorrectos</response>
        /// <response code="404">No se encuentra perfil</response>
        /// <response code="500">Error de servidor</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutPerfil(int id, Perfil perfil)
        {
            if (id != perfil.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(perfil).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerfilExists(id))
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

        /// <summary>Añadir un nuevo perfil</summary>
        /// <param name="perfil">Perfil</param>
        /// <response code="200">Perfil añadido</response>
        /// <response code="409">Perfil ya existente</response>
        /// <response code="500">Error de servidor</response>
        [HttpPost]
        public async Task<ActionResult<Perfil>> PostPerfil(Perfil perfil)
        {
            _contexto.Perfiles.Add(perfil);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction("GetPerfiles", new { id = perfil.Id }, perfil);
        }

        /// <summary>Borrar un perfil</summary>
        /// <param name="id">Id de perfil</param>
        /// <response code="200">Perfil borrado</response>
        /// <response code="404">No se encuentra perfil</response>
        /// <response code="500">Error de servidor</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Perfil>> DeletePerfil(int id)
        {
            var perfil = await _contexto.Perfiles.FindAsync(id);
            if (perfil == null)
            {
                return NotFound();
            }

            _contexto.Perfiles.Remove(perfil);
            await _contexto.SaveChangesAsync();

            return perfil;
        }

        //Comprobar si existe un perfil
        private bool PerfilExists(int id)
        {
            return _contexto.Perfiles.Any(e => e.Id == id);
        }
    }
}
