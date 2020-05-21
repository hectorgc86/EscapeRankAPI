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
    public class CategoriasController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public CategoriasController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/categorias
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

        // GET: api/categorias/5
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

        // PUT: api/categorias/5
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

        // POST: api/categorias
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

        // DELETE: api/categorias/5
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

        private bool CategoriaExists(string id)
        {
            return _contexto.Categorias.Any(e => e.Id == id);
        }
    }
}
