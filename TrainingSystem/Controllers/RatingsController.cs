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
    [Route("api/Ratings")]
    public class RatingsController : Controller
    {
        private readonly TrainingSystemContext _context;

        public RatingsController(TrainingSystemContext context)
        {
            _context = context;
        }

        // GET: api/Ratings
        [HttpGet]
        public IEnumerable<Rating> GetRatings()
        {
            return _context.Rating.Include(r => r.Course);
        }

        // PUT: api/Ratings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRating([FromRoute] int id, [FromBody] Rating rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rating.RatingId)
            {
                return BadRequest();
            }

            _context.Entry(rating).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RatingExists(id))
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

        private bool RatingExists(int id)
        {
            return _context.Rating.Any(r => r.RatingId == id);
        }
    }
}