using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingSystem.Data;
using TrainingSystem.Models;
using TrainingSystem.ViewModels;

namespace TrainingSystem.Controllers
{
    [Produces("application/json")]
    [Route("api/Materials")]
    public class MaterialsController : Controller
    {
        private readonly TrainingSystemContext _context;
        private readonly IHostingEnvironment _environment;

        public MaterialsController(TrainingSystemContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // POST: api/Materials
        [HttpPost]
        public async Task<IActionResult> PostMaterial([FromForm] MaterialViewModel materialVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filePath = Path.Combine(_environment.ContentRootPath, @"Uploads", materialVm.File.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await materialVm.File.CopyToAsync(stream);
            }

            var material = new Material
            {
                Name = materialVm.Name,
                FileName = materialVm.File.FileName,
                CourseId = materialVm.CourseId
            };

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

            var filePath = Path.Combine(_environment.ContentRootPath, @"Uploads", material.FileName);
            System.IO.File.Delete(filePath);

            _context.Material.Remove(material);
            await _context.SaveChangesAsync();

            return Ok(material);
        }

        private bool MaterialExists(int id)
        {
            return _context.Material.Any(m => m.MaterialId == id);
        }
    }
}