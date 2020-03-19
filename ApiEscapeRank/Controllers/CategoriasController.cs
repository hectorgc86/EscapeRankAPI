using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiEscapeRank
{
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
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            return await _contexto.Categorias.ToListAsync();
        }

        // GET: api/categorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(string id)
        {
            var categoria = await _contexto.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        // PUT: api/categorias/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(string id, Categoria categoria)
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
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
            var categoria = await _contexto.Categorias.FindAsync(id);
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