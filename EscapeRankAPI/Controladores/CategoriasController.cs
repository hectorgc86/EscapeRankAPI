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
    public class CategoriasController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public CategoriasController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>Obtener todas las categorías</summary>
        /// <response code="200">Categorías devueltas</response>
        /// <response code="404">No se encuentran categorías</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> GetCategorias()
        {
            List<Categoria> categorias = await _contexto.GetCategorias().ToListAsync();

            if (categorias == null)
            {
                return NotFound();
            }

            return categorias;
        }

        /// <summary>Obtener una categoría por su id</summary>
        /// <param name="id">Id de categoría</param>
        /// <response code="200">Categoría devuelta</response>
        /// <response code="404">No se encuentra categoría</response>
        /// <response code="500">Error de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(string id)
        {
            Categoria categoria = await _contexto.GetCategoria(id).FirstOrDefaultAsync();

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        /// <summary>Modificar una categoría por su id</summary>
        /// <param name="id">Id de categoría a modificar</param>
        /// <param name="categoria">Categoría modificada    </param>
        /// <response code="200">Categoría modificada</response>
        /// <response code="400">Parámetros incorrectos</response>
        /// <response code="404">No se encuentra categoría</response>
        /// <response code="500">Error de servidor</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCategoria(string id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
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

        /// <summary>Añadir una nueva categoría</summary>
        /// <param name="categoria">Categoría</param>
        /// <response code="200">Categoría añadida</response>
        /// <response code="409">Categoría ya existente</response>
        /// <response code="500">Error de servidor</response>
        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
        {
            _contexto.Categorias.Add(categoria);

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CategoriaExists(categoria.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCategoria", new { id = categoria.Id }, categoria);
        }

        /// <summary>Borrar una categoría</summary>
        /// <param name="id">Id de categoría</param>
        /// <response code="200">Categoría borrada</response>
        /// <response code="404">No se encuentra categoría</response>
        /// <response code="500">Error de servidor</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Categoria>> DeleteCategoria(string id)
        {
            Categoria categoria = await _contexto.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            _contexto.Categorias.Remove(categoria);
            await _contexto.SaveChangesAsync();

            return categoria;
        }

        //Comprobar si existe una categoría
        private bool CategoriaExists(string id)
        {
            return _contexto.Categorias.Any(e => e.Id == id);
        }
    }
}
