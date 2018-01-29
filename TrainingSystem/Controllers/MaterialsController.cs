using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingSystem.Data;
using TrainingSystem.Models;

namespace TrainingSystem.Controllers
{
    [Produces("application/json")]
    [Route("api/Materials")]
    public class MaterialsController : Controller
    {
        private readonly TrainingSystemContext _context;

        public MaterialsController(TrainingSystemContext context)
        {
            _context = context;
        }

        // GET: api/Materials
        [HttpGet]
        public IEnumerable<Material> GetMaterial()
        {
            return _context.Material;
        }

        // GET: api/Materials/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMaterial([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var material = await _context.Material.SingleOrDefaultAsync(m => m.MaterialId == id);

            if (material == null)
            {
                return NotFound();
            }

            return Ok(material);
        }

        // PUT: api/Materials/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterial([FromRoute] int id, [FromBody] Material material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != material.MaterialId)
            {
                return BadRequest();
            }

            _context.Entry(material).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(id))
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

        // POST: api/Materials
        [HttpPost]
        public async Task<IActionResult> PostMaterial([FromBody] Material material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Material.Add(material);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaterial", new { id = material.MaterialId }, material);
        }

        // DELETE: api/Materials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var material = await _context.Material.SingleOrDefaultAsync(m => m.MaterialId == id);
            if (material == null)
            {
                return NotFound();
            }

            _context.Material.Remove(material);
            await _context.SaveChangesAsync();

            return Ok(material);
        }

        private bool MaterialExists(int id)
        {
            return _context.Material.Any(e => e.MaterialId == id);
        }
    }
}