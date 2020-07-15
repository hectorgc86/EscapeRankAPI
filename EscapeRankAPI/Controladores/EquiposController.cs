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
    public class EquiposController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public EquiposController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>Obtener todos los equipos</summary>
        /// <response code="200">Equipos devueltos</response>
        /// <response code="404">No se encuentran equipos</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet]
        public async Task<ActionResult<List<Equipo>>> GetEquipos()
        {
            List<Equipo> equipos = await _contexto.GetEquipos().ToListAsync();

            if (equipos == null)
            {
                return NotFound();
            }

            return equipos;
        }

        /// <summary>Obtener todos los equipos a los que un usuario pertenece</summary>
        /// <param name="id">Id de usuario</param>
        /// <response code="200">Equipos de usuario devueltos</response>
        /// <response code="404">No se encuentran equipos</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("usuario/{id}")]
        public async Task<ActionResult<List<Equipo>>> GetEquiposUsuario(int id)
        {

            List<Equipo> equipoUsuario = await _contexto.GetEquiposUsuario(id).ToListAsync();

            if (equipoUsuario == null)
            {
                return NotFound();
            }
            else
            {
                return equipoUsuario;
            }
        }

        /// <summary>Obtener un equipo por su id</summary>
        /// <param name="id">Id de equipo</param>
        /// <response code="200">Equipo devuelto</response>
        /// <response code="404">No se encuentra equipo</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipo>> GetEquipo(int id)
        {
            Equipo equipo = await _contexto.GetEquipo(id).FirstOrDefaultAsync();

            if (equipo == null)
            {
                return NotFound();
            }

            return equipo;
        }

        /// <summary>Modificar un equipo por su id</summary>
        /// <param name="id">Id de equipo a modificar</param>
        /// <param name="equipo">Equipo modificado</param>
        /// <response code="200">Equipo modificado</response>
        /// <response code="400">Parámetros incorrectos</response>
        /// <response code="404">No se encuentra equipo</response>
        /// <response code="500">Error de servidor</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutEquipo(int id, Equipo equipo)
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

        /// <summary>Añadir un nuevo equipo</summary>
        /// <param name="req">Equipo</param>
        /// <response code="200">Equipo añadido</response>
        /// <response code="409">Equipo ya existente</response>
        /// <response code="500">Error de servidor</response>
        [HttpPost]
        public async Task<ActionResult> PostEquipo(EquipoRequest req)
        {
            Equipo equipo = new Equipo
            {
                Nombre = req.Nombre,
                Avatar = "https://picsum.photos/200/300?random=",
                Activado = true
            };

            _contexto.Equipos.Add(equipo);

            foreach (Usuario u in req.Usuarios){

                EquiposUsuarios eu = new EquiposUsuarios
                {
                    UsuarioId = u.Id,
                    EquipoId = equipo.Id
                };

                equipo.EquiposUsuarios.Add(eu);
            }

            _contexto.Equipos.Add(equipo);

            try
            {
                await _contexto.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                throw;
            }

        }

        /// <summary>Borrar un equipo</summary>
        /// <param name="id">Id de equipo</param>
        /// <response code="200">Equipo borrado</response>
        /// <response code="404">No se encuentra equipo</response>
        /// <response code="500">Error de servidor</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEquipo(int id)
        {
            Equipo equipo = await _contexto.GetEquipo(id).FirstOrDefaultAsync();

            if (equipo == null)
            {
                return NotFound();
            }

            equipo.Activado = false;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
           
            return Ok();
        }

        //Comprobar si existe un equipo
        private bool EquipoExists(int id)
        {
            return _contexto.Equipos.Any(e => e.Id == id);
        }
    }
}
