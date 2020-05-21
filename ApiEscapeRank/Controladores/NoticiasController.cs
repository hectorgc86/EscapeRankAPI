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
        public async Task<ActionResult<List<Noticia>>> GetNoticias()
        {
            List<Noticia> noticias = await _contexto.GetNoticias().ToListAsync();

            if(noticias == null)
            {
                return NotFound();
            }

            return noticias;
        }

        // GET: api/noticias/usuario/5
        [HttpGet("usuario/{id}")]
        public async Task<ActionResult<List<Noticia>>> GetNoticiasUsuario(int id)
        {
            List<Noticia> noticiasUsuario = await _contexto.GetNoticiasUsuario(id).ToListAsync();

            if (noticiasUsuario == null)
            {
                return NotFound();
            }
            else
            {
                return noticiasUsuario;
            }
        }

        // GET: api/noticias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Noticia>> GetNoticia(int id)
        {
            Noticia noticia = await _contexto.GetNoticia(id).FirstOrDefaultAsync();

            if (noticia == null)
            {
                return NotFound();
            }

            return noticia;
        }

        // PUT: api/noticias/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutNoticia(int id, Noticia noticia)
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
