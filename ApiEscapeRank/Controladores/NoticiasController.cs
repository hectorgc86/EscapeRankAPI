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
    public class NoticiasController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public NoticiasController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/noticias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Noticia>>> GetNoticias()
        {
            return await _contexto.Noticias.ToListAsync();
        }

        // GET: api/noticias/usuario/5
        [HttpGet("usuario/{id}")]
        public async Task<ActionResult<List<Noticia>>> GetNoticiasUsuario(int id)
        {
            string consulta = "SELECT * FROM noticias WHERE usuario_id" +
                " IN(SELECT amigo_id FROM usuarios_amigos WHERE usuario_id = " + id + ")" +
                " OR noticias.promocionada = 1" +
                " OR noticias.usuario_id = " + id + " ORDER by noticias.fecha DESC";

            List<Noticia> noticias = await _contexto.Noticias.FromSqlRaw(consulta).ToListAsync();

            if (noticias == null)
            {
                return NotFound("No se encuentran noticias para usuario con id " + id);
            }
            else
            {
                return noticias;
            }
        }

        // GET: api/noticias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Noticia>> GetNoticia(int id)
        {
            var noticia = await _contexto.Noticias.FindAsync(id);

            if (noticia == null)
            {
                return NotFound();
            }

            return noticia;
        }

        // PUT: api/noticias/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNoticia(int id, Noticia noticia)
        {
            if (id != noticia.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(noticia).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoticiaExists(id))
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

        // POST: api/noticias
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Noticia>> PostNoticia(Noticia noticia)
        {
            _contexto.Noticias.Add(noticia);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction("GetNoticias", new { id = noticia.Id }, noticia);
        }

        // DELETE: api/noticias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Noticia>> DeleteNoticia(int id)
        {
            var noticia = await _contexto.Noticias.FindAsync(id);
            if (noticia == null)
            {
                return NotFound();
            }

            _contexto.Noticias.Remove(noticia);
            await _contexto.SaveChangesAsync();

            return noticia;
        }

        private bool NoticiaExists(int id)
        {
            return _contexto.Noticias.Any(e => e.Id == id);
        }
    }
}
