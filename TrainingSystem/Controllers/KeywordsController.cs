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
    [Route("api/Keywords")]
    public class KeywordsController : Controller
    {
        private readonly TrainingSystemContext _context;

        public KeywordsController(TrainingSystemContext context)
        {
            _context = context;
        }

        // GET: api/Keywords
        [HttpGet]
        public IEnumerable<Keyword> GetKeywords()
        {
            return _context.Keyword.Include(k => k.Course);
        }

        // GET: api/Keywords/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetKeyword([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var keyword = await _context.Keyword
                .Include(k => k.Course)
                .SingleOrDefaultAsync(k => k.KeywordId == id);

            if (keyword == null)
            {
                return NotFound();
            }

            return Ok(keyword);
        }

        // POST: api/Keywords
        [HttpPost]
        public async Task<IActionResult> PostKeyword([FromBody] Keyword keyword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Keyword.Add(keyword);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKeyword", new { id = keyword.KeywordId }, keyword);
        }
    }
}