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
    public class NoticiasController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public NoticiasController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>Obtener todas las noticias</summary>
        /// <response code="200">Noticias devueltos</response>
        /// <response code="404">No se encuentran noticias</response>
        /// <response code="500">Error de servidor</response>
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

        /// <summary>Obtener todas las noticias de un usuario</summary>
        /// <param name="id">Id de usuario</param>
        /// <response code="200">Noticias de usuario devueltas</response>
        /// <response code="404">No se encuentran noticias</response>
        /// <response code="500">Error de servidor</response>
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

        /// <summary>Obtener una noticia por su id</summary>
        /// <param name="id">Id de noticia</param>
        /// <response code="200">Noticia devuelta</response>
        /// <response code="404">No se encuentra noticia</response>
        /// <response code="500">Error de servidor</response>
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

        /// <summary>Modificar una noticia por su id</summary>
        /// <param name="id">Id de noticia a modificar</param>
        /// <param name="noticia">Noticia modificada</param>
        /// <response code="200">Noticia modificada</response>
        /// <response code="400">Parámetros incorrectos</response>
        /// <response code="404">No se encuentra noticia</response>
        /// <response code="500">Error de servidor</response>
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

        /// <summary>Añadir una nueva noticia</summary>
        /// <param name="noticia">Noticia</param>
        /// <response code="200">Noticia añadida</response>
        /// <response code="409">Noticia ya existente</response>
        /// <response code="500">Error de servidor</response>
        [HttpPost]
        public async Task<ActionResult<Noticia>> PostNoticia(Noticia noticia)
        {
            _contexto.Noticias.Add(noticia);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction("GetNoticias", new { id = noticia.Id }, noticia);
        }

        /// <summary>Borrar una noticia</summary>
        /// <param name="id">Id de noticia</param>
        /// <response code="200">Noticia borrada</response>
        /// <response code="404">No se encuentra noticia</response>
        /// <response code="500">Error de servidor</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Noticia>> DeleteNoticia(int id)
        {
            Noticia noticia = await _contexto.Noticias.FindAsync(id);
            if (noticia == null)
            {
                return NotFound();
            }

            _contexto.Noticias.Remove(noticia);
            await _contexto.SaveChangesAsync();

            return noticia;
        }

        //Comprobar si existe una noticia
        private bool NoticiaExists(int id)
        {
            return _contexto.Noticias.Any(e => e.Id == id);
        }
    }
}
