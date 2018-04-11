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

        // POST: api/Ratings 
        [HttpPost]
        public async Task<IActionResult> PostRating([FromBody] Rating rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Rating.Add(rating);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRating", new { id = rating.RatingId }, rating);
        }

        private bool RatingExists(int id)
        {
            return _context.Rating.Any(r => r.RatingId == id);
        }
    }
}