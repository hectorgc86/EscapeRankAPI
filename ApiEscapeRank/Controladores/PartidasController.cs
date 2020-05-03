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
    public class PartidasController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public PartidasController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/partidas
        [HttpGet]
        public async Task<ActionResult<List<Partida>>> GetPartidas()
        {
            return await _contexto.Partidas.ToListAsync();
        }

        // GET: api/partidas/usuario/5
        [HttpGet("usuario/{id}")]
        public async Task<ActionResult<List<Partida>>> GetPartidasUsuario(int id)
        {
            string sqlString = "SELECT * from partidas WHERE equipo_id IN (SELECT equipo_id FROM equipos_usuarios WHERE usuario_id = "+ id +")";

            List<Partida> partidasUsuario = await _contexto.Partidas.FromSqlRaw(sqlString)
                .Include(s=>s.Sala)
                .Include(e=>e.Equipo)
                .OrderByDescending(f=>f.Fecha)
                .ToListAsync();

            if (partidasUsuario == null)
            {
                return NotFound();
            }

            return partidasUsuario;
        }

        // GET: api/partidas/equipo/5
        [HttpGet("equipo/{id}")]
        public async Task<ActionResult<List<Partida>>> GetPartidasEquipo(int id)
        {
            List<Partida> partidasEquipo = await _contexto.Partidas
                .Where(ei => ei.EquipoId == id)
                .Include(s => s.Sala)
                .ToListAsync();

            if (partidasEquipo == null)
            {
                return NotFound();
            }

            return partidasEquipo;
        }

        // GET: api/partidas/sala/5
        [HttpGet("sala/{id}")]
        public async Task<ActionResult<List<Partida>>> GetPartidasSala(string id)
        {
            string sqlString = "SELECT * FROM partidas WHERE sala_id = '" + id + "'";

            List<Partida> partidasSala = await _contexto.Partidas
                .Where(si=>si.SalaId == id)
                .Include(s => s.Sala)
                .Include(e=>e.Equipo)
                .ToListAsync();

            if (partidasSala == null)
            {
                return NotFound();
            }

            return partidasSala;
        }

        // GET: api/partidas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Partida>> GetPartida(int id)
        {
            var partida = await _contexto.Partidas.FindAsync(id);

            if (partida == null)
            {
                return NotFound();
            }

            return partida;
        }

        // PUT: api/partidas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPartida(int id, Partida partida)
        {
            if (id != partida.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(partida).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartidaExists(id))
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

        // POST: api/partidas
        [HttpPost]
        public async Task<ActionResult<bool>> PostPartida(PartidaRequest req)
        {
            Partida partidaNueva = new Partida
            {
                Minutos = req.Minutos,
                Segundos = req.Segundos,
                SalaId = req.Sala.Id,
                EquipoId = req.Equipo.Id,
                Fecha = req.Fecha
            };

            _contexto.Partidas.Add(partidaNueva);

            try
            {
                await _contexto.SaveChangesAsync();

                Noticia n = new Noticia
                {
                    Titular = "Partida jugada en " + req.Sala.Nombre,
                    TextoCorto = "Has jugado una partida con tu equipo: " + req.Equipo.Nombre,
                    TextoLargo = "Sala realizada en un tiempo de " + partidaNueva.Minutos + " minutos con " + partidaNueva.Segundos + "segundos el día " + partidaNueva.Fecha +". El equipo de EscapeRank y de la sala " + req.Sala.Nombre + " te estamos muy agradecidos."
                };

                string sqlString = "SELECT * FROM usuarios WHERE id IN (SELECT usuario_id FROM equipos_usuarios WHERE equipo_id = " + partidaNueva.EquipoId + ")";

                List<Usuario> usuariosEquipo = await _contexto.Usuarios.FromSqlRaw(sqlString).ToListAsync();

                foreach(Usuario u in usuariosEquipo)
                {
                    u.Noticias.Add(n);
                }

                await _contexto.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        // DELETE: api/Partidas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Partida>> DeletePartida(int id)
        {
            var partida = await _contexto.Partidas.FindAsync(id);
            if (partida == null)
            {
                return NotFound();
            }

            _contexto.Partidas.Remove(partida);
            await _contexto.SaveChangesAsync();

            return partida;
        }

        private bool PartidaExists(int id)
        {
            return _contexto.Partidas.Any(e => e.Id == id);
        }
    }
}
