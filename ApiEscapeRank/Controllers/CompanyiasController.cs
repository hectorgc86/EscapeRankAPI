using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiEscapeRank
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyiasController : ControllerBase
    {
        private readonly MySQLDbcontext _contexto;

        public CompanyiasController(MySQLDbcontext contexto)
        {
            _contexto = contexto;
        }

        // GET: api/companyias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Companyia>>> GetCompanyias()
        {
            return await _contexto.Companyias.ToListAsync();
        }

        // GET: api/companyias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Companyia>> GetCompanyia(string id)
        {
            var companyias = await _contexto.Companyias.FindAsync(id);

            if (companyias == null)
            {
                return NotFound();
            }

            return companyias;
        }

        // PUT: api/companyias/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompanyia(string id, Companyia companyias)
        {
            if (id != companyias.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(companyias).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyiaExists(id))
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

        // POST: api/companyias
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Companyia>> PostCompanyia(Companyia companyia)
        {
            _contexto.Companyias.Add(companyia);
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CompanyiaExists(companyia.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCompanyias", new { id = companyia.Id }, companyia);
        }

        // DELETE: api/companyias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Companyia>> DeleteCompanyia(string id)
        {
            var companyia = await _contexto.Companyias.FindAsync(id);
            if (companyia == null)
            {
                return NotFound();
            }

            _contexto.Companyias.Remove(companyia);
            await _contexto.SaveChangesAsync();

            return companyia;
        }

        private bool CompanyiaExists(string id)
        {
            return _contexto.Companyias.Any(e => e.Id == id);
        }
    }
}
