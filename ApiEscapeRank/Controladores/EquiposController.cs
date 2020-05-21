using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiEscapeRank.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiEscapeRank.Controladores
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

        // GET: api/equipos
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

        // GET: api/equipos/usuario/5
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

        // GET: api/equipos/5
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

        // PUT: api/equipos/5
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

        // POST: api/equipos
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

        // DELETE: api/equipos/5
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

        private bool EquipoExists(int id)
        {
            return _contexto.Equipos.Any(e => e.Id == id);
        }
    }
}
